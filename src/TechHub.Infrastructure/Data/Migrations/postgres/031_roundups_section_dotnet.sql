-- Migration 031: Clean roundup INSERTs for section 'dotnet'
-- Generated: 2026-05-19 from live database state.
-- Replaces the two-step migrations 028 (backfill) + 029 (section-focus update)
-- with a single idempotent INSERT per roundup row using ON CONFLICT DO UPDATE.
-- Safe to re-run: duplicate slugs will update to the same values.

-- ── content_items ─────────────────────────────────────────────────────────
-- weekly-dotnet-roundup-2026-05-11
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', 'Agent Governance, Safer Ops, and Platform Modernization',
    'This week in .NET, Microsoft kept pushing the platform in two directions at once: modernizing long-running enterprise workloads (including mainframe connectivity) while tightening the inner loop for web and client apps with better testing, faster WebAssembly, and more pragmatic API design patterns. That maps cleanly to last week''s split between "ship-ready platform updates" and "what''s taking shape next." .NET 10 keeps showing up as the standardization point (from Ubuntu 26.04 baselines and container tags to real products migrating runtimes), while .NET 11 previews continue to fill in practical workflow gaps (Blazor UX and testing) before they harden into defaults.

<!--excerpt_end-->

## .NET

### .NET 10 adoption in real products (Host Integration Server and WebAssembly)

Following last week''s focus on .NET 10 as a new baseline on Ubuntu 26.04 (plus the matching `resolute` container tags for cleaner Linux standardization), Host Integration Server 2028 preview shows what "adoption" looks like when a long-lived enterprise product moves its core onto .NET 10. Microsoft is modernizing IBM connectivity by pairing that runtime move with new integration surfaces and platform cleanups. The headline for many teams will be expanded REST API surfaces for DB2 and for transaction integration scenarios (CICS and IMS), which shifts common host connectivity patterns closer to the HTTP-first tooling and operational model teams already use for modern services. On the ops and security side, the preview adds Microsoft Entra ID support for identity and Azure Arc support for hybrid management, aiming to make host-connected deployments fit better into current governance and inventory practices rather than living as a separate island. Microsoft is also using the release to deprecate legacy components and remove older dependencies, and it calls out Linux support for non-SNA features as part of the .NET 10-based direction (a practical signal if you are planning mixed OS deployments but still rely on specific protocol stacks). There is even an Azure AI Foundry integration callout, framing host data and transactions as candidates for AI-assisted experiences and workflows, not just back-office plumbing.

In a very different corner of the stack, Copilot Studio moved its .NET WebAssembly runtime from .NET 8 to .NET 10 and documented what that migration buys in practice: tighter defaults and measurable perf wins when you lean into ahead-of-time (AOT) compilation. This also connects back to last week''s Linux baseline story, where Native AOT came up as part of making deployments leaner and more predictable. The post highlights built-in asset fingerprinting (useful for long-lived caching without serving stale bits) and a new default behavior around `WasmStripILAfterAOT`, which reduces what ships to the client after AOT. Taken together, these updates show .NET 10 maturing as a runtime you can standardize on for both hybrid enterprise integration servers and browser-hosted apps, with improvements landing not just in APIs but in the build and deployment output that affects cold start, download size, and caching behavior.

- [Announcing Microsoft Host Integration Server 2028: Modern connectivity for IBM Mainframes Midranges](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/announcing-microsoft-host-integration-server-2028-modern/ba-p/4517606)
- [Copilot Studio gets faster with .NET 10 on WebAssembly](https://devblogs.microsoft.com/dotnet/copilot-studio-dotnet-10-migration/)

### ASP.NET Core and Blazor: testing and versioning getting more concrete

Blazor’s tooling story took a step toward more realistic UI validation with a first look at a new end-to-end (E2E) component testing library, previewed in .NET 11. Coming right after last week''s .NET 11 direction-setting around Blazor validation and "move common plumbing into the framework," this is another push toward making core app workflows (forms, lists, and now testing) less dependent on custom harnesses and one-off test infrastructure. In the Community Standup, Daniel Roth and Javier Calvarro Nelson walked through the motivation and demoed the approach against real apps, positioning it as a way to test components in scenarios closer to how users actually interact with Blazor UIs (instead of stopping at unit tests or framework-specific abstractions). The key point for teams planning upgrades is timing: it is framed as a .NET 11 preview with explicit next steps driven by community feedback, so this is a good moment to watch the API shape and align internal testing patterns before it hardens.

On the API side, a practical .NET 10 minimal API tutorial showed how to add versioning with `Asp.Versioning.Http`, including both query-string versioning and URL-segment versioning. This pairs naturally with last week''s standup on aligning API versioning with OpenAPI output in .NET 10: once you start versioning minimal APIs, the next pain point is keeping docs and client generation aligned without duplicating configuration. Beyond the mechanics, it covers how to deprecate an API version, which matters when you are trying to keep minimal APIs minimal without painting yourself into a compatibility corner. The walkthrough uses a Visual Studio 2026 `.http` file for testing, reinforcing a workflow where versioning behavior is easy to validate in-repo without needing a heavier client setup.

- [Blazor Community Standup: E2E Component Testing for Blazor](https://www.youtube.com/watch?v=C1e1_7G1VaE)
- [Add versioning to .NET 10 minimal API using Asp.Versioning.Http](https://www.youtube.com/watch?v=y2zzX2aOx8Q)

### .NET MAUI: cross-language UI experiments and on-device processing demos

The latest .NET MAUI Community Standup leaned into experimentation across the UI and native boundary. In context, it feels like the "client app inner loop" thread from last week (where MauiDevFlow focused on faster build-deploy-inspect cycles and better inspection hooks) expanding into "what else can we wire into MAUI to unlock new app patterns." David Ortinau and Gerald Versluis, joined by Nick Kovalsky, demoed scenarios that mix Rust with .NET MAUI, explored SkiaSharp in broader "everywhere" usage, and discussed a drawn-UI approach where custom rendering can offer tighter control over visuals and performance than standard widget composition. The session also touched AI/ML live processing work, pointing to app patterns where you process streams (like camera, audio, or sensor input) continuously rather than treating ML as a batch step. For MAUI teams, the takeaway is less about a single shipped feature and more about where the ecosystem is spending energy: interoperability, custom rendering pipelines, and practical ML workloads inside real client apps.

- [.NET MAUI Community Standup: Rust, SkiaSharp Everywhere, AI/ML Live Processing](https://www.youtube.com/watch?v=mfj8oxjqdaM)

### Other .NET News

Microsoft launched "Quest to Compile", a new show focused on modern game development in the .NET ecosystem, covering core topics (gameplay programming, debugging) alongside day-to-day workflows like Git and AI-assisted development with GitHub Copilot.

- [Introducing Quest to Compile: A Show for Game Devs Building in .NET](https://www.youtube.com/watch?v=RpvqkzdaUD8)

Mark Russinovich explained why Win32 is still treated as a first-class API surface in 2026, tying it directly to Windows'' deep Win32 foundations and the practical reality that long-term compatibility supports a large existing app ecosystem.

- [Did anyone in the ’90s expect Win32 to still be a first‑class API surface in 2026?](https://www.youtube.com/watch?v=FNZJnpvgkQM)

Amanda Silver revisited the origin of TypeScript, grounding it in the need to make large JavaScript codebases more scalable and maintainable through stronger structure and developer tooling support.

- [Why was TypeScript created?](https://www.youtube.com/watch?v=I2Ej8q2DDSw)',
    'This week in .NET, Microsoft kept pushing the platform in two directions at once: modernizing long-running enterprise workloads (including mainframe connectivity) while tightening the inner loop for web and client apps with better testing, faster WebAssembly, and more pragmatic API design patterns. That maps cleanly to last week''s split between "ship-ready platform updates" and "what''s taking shape next." .NET 10 keeps showing up as the standardization point (from Ubuntu 26.04 baselines and container tags to real products migrating runtimes), while .NET 11 previews continue to fill in practical workflow gaps (Blazor UX and testing) before they harden into defaults.',
    1778482800, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2026-05-11', 'TechHub',
    'TechHub', '61efed4332db4e6588ba88cca41910db', ',GitHub Copilot,VS Code,Copilot CLI,Model Governance,OpenTelemetry,BYOK,MCP,Azure AI Foundry,Microsoft Agent Framework,Microsoft Fabric,Databricks,GitHub Enterprise,Repository Rulesets,Azure Virtual Machines,Microsoft Defender,AI,Azure,DevOps,.NET,ML,Security,Roundups,',
    false, false, true,
    false, false,
    false, false, 4, '{"key_topics": ["GitHub Copilot", "VS Code", "Copilot CLI", "Model Governance", "OpenTelemetry", "BYOK", "MCP", "Azure AI Foundry", "Microsoft Agent Framework", "Microsoft Fabric", "Databricks", "GitHub Enterprise", "Repository Rulesets", "Azure Virtual Machines", "Microsoft Defender", "AI", "Azure", "DevOps", ".NET", "ML", "Security"], "topic_type": "news", "roundup_summary": "This week focused on making AI agents more controllable in real workflows, from Copilot context improvements and tracing in VS Code to enterprise-managed CLI plugins, model deprecation deadlines, and clearer review hygiene for agent-authored PRs. Microsoft advanced production agent building and deployment with the Agent Framework and Azure AI Foundry, while Fabric and Databricks shipped practical operations features for discoverability, concurrency, monitoring, and recovery. Security and governance news emphasized token theft defense, passkey rollout realities, and shifting scanning earlier into agent tools with GitHub MCP Server and runtime-aware code-to-cloud visibility.", "roundup_relevance": "high"}'::jsonb
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2026-05-04
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'Token Billing, Interoperable Agents, and Practical Cloud Controls',
    'This week in .NET was a mix of platform plumbing and practical building blocks: Microsoft pushed forward on modernizing the toolchain (especially inside Visual Studio), while several posts showed how .NET 10+ apps are increasingly composed from focused libraries for AI, caching, and API surface management. Coming right after last week''s split between "install the preview" (.NET 11 Preview 3) and "patch production now" (April 2026 servicing), the throughline is familiar: the platform keeps tightening defaults (dependencies, provenance, project systems), and teams need to validate those shifts early to avoid surprises later. At the same time, a couple of changes signaled where the ecosystem is heading next, including a notable test platform dependency shift that could surface as a breaking change in CI.

<!--excerpt_end-->

## .NET

### Visual Studio and the .NET toolchain are getting more "SDK-style" (with a few sharp edges)

Visual Studio 18.5 added official SDK-style project support for VSSDK-based VSIX extension projects, which is a meaningful quality-of-life improvement if you maintain extensions that have historically lived in older project system land. The headline is that SDK-style enables better incremental builds, including the Fast Up To Date Check, so inner-loop iteration on extensions should feel closer to modern .NET projects. Microsoft also shipped updated templates and provided a short migration checklist to help existing extensions move over without guesswork, which matters because extension projects tend to accrete custom MSBuild logic over time. Seen next to last week''s SDK/CLI "inner loop" focus in .NET 11 Preview 3 (dotnet watch resiliency, CLI edits for solution filters, and other iteration tweaks), this is another step in the same direction: more of the ecosystem moving to the newer project system conventions so tooling can be faster and more predictable.

In the same "tooling modernization" vein, VSTest announced it is removing its Newtonsoft.Json dependency starting in .NET 11 Preview 4 and Visual Studio 18.8. The new default is System.Text.Json, while .NET Framework scenarios will use JSONite. This is partly motivated by keeping dependencies tighter and responding to ecosystem pressure around NuGet vulnerabilities, but it has real compatibility implications: adapters, collectors, and any test infrastructure that assumed Newtonsoft.Json was present transitively may fail at runtime. The guidance is straightforward but important for extension authors and test tooling maintainers: explicitly reference Newtonsoft.Json if you still need it, and check any serialization behaviors that change when moving to System.Text.Json. It is also a clear continuation of last week''s "keep your inputs trustworthy" theme (signed official .NET container images, servicing patches, and deadlines that force modernization): the platform is steadily removing implicit assumptions and nudging teams toward explicit dependencies and maintained defaults.

- [SDK-Style Support for Extension Projects](https://devblogs.microsoft.com/visualstudio/sdk-style-support-for-extension-projects/)
- [VSTest is Removing its Newtonsoft.Json Dependency](https://devblogs.microsoft.com/dotnet/vs-test-is-removing-its-newtonsoft-json-dependency/)

### SkiaSharp 4.0 Preview 1 modernizes graphics primitives and expands targets

SkiaSharp 4.0 Preview 1 landed with a jump to Skia milestone 147 and a focus on newer typography and drawing APIs. Variable fonts (OpenType variable fonts) and color font palettes are the kind of features you only notice once you try to render modern design systems across platforms, and having them supported at the library level reduces the need for platform-specific workarounds. The preview also introduces SKPathBuilder, which should make constructing complex paths more ergonomic and potentially less error-prone than manual path mutation patterns.

On the platform side, the release adds new native targets and ships an interactive gallery implemented with Blazor WebAssembly, which is a practical way to let developers verify rendering behavior quickly without pulling down a full sample solution. The post also calls out Uno Platform co-maintenance, continuing the theme that SkiaSharp sits at the intersection of Microsoft and cross-platform UI communities. With last week''s .NET 11 Preview 3 spending time on browser/WASM packaging and debugging (including WebCIL) and on UI ergonomics like Blazor Virtualize improvements, SkiaSharp''s Blazor WASM gallery fits the broader pattern: .NET-in-the-browser tooling is becoming a more normal part of how libraries demonstrate and validate cross-platform behavior.

- [Welcome to SkiaSharp 4.0 Preview 1](https://devblogs.microsoft.com/dotnet/welcome-to-skia-sharp-40-preview1/)

### Composable AI in .NET: a reference app that stitches together the stack

ConferencePulse is a good example of how Microsoft wants teams to build AI features in .NET: not via one monolithic framework, but by composing smaller pieces. The tutorial walks through using Microsoft.Extensions.AI as the application-facing abstraction, then layering in DataIngestion and VectorData for retrieval-augmented generation (RAG)-style Q&A. It also pulls in MCP (Model Context Protocol) and the Microsoft Agent Framework to drive tool-based workflows like poll generation, plus multi-agent session summaries and real-time insights.

What makes the walkthrough useful is that it is not only about prompting. It treats ingestion, vector storage, and tool execution as first-class parts of the app, and it runs under .NET Aspire with Azure OpenAI, so you can see how the pieces hang together in a real deployment shape (including OpenTelemetry for observing the system). If you''re deciding whether to adopt these new "Extensions.*" AI packages, this is one of the clearer end-to-end examples of how they are intended to be wired. It also mirrors last week''s preview theme of "small primitives that reduce custom glue" (for example, more built-in knobs in System.Text.Json and other BCL areas): the composable approach is showing up across the stack, not just in AI.

- [Building an AI-Powered Conference App with .NET’s Composable AI Stack](https://devblogs.microsoft.com/dotnet/building-ai-conference-app-dotnet-composable-stack/)

### Performance and interop patterns: tiered caching and Native AOT DLL exports

On the performance side, Microsoft demonstrated tiered caching in a .NET 10 app using HybridCache with Azure Database for PostgreSQL as the distributed cache (via Microsoft.Extensions.Caching.Postgres). The core idea is to combine an in-memory layer for fast hits with a shared Postgres-backed layer for cross-instance reuse, then measure the difference between cold and warm paths. The tutorial stays grounded in implementation details you actually need in a real service: configuration, secrets handling with dotnet user-secrets, and a simple benchmark setup that highlights lower latency on cache hits once the pipeline is warmed. Last week''s .NET 11 Preview 3 leaned heavily into runtime/JIT optimizations and hot-path wins without code changes, and this caching walkthrough complements that story from the app architecture side: even as the runtime trims overhead, teams still get big, measurable wins by designing for warm paths and shared state across instances.

For interop-heavy teams, Rick Strahl showed how .NET Native AOT can publish a Windows-native WinAPI-style DLL from a .NET 10+ class library, including exporting functions via UnmanagedCallersOnly and publishing with dotnet publish. The guide is candid about constraints: when you want to be callable from non-.NET code (like older tools such as FoxPro), you have to think in terms of stable exported entry points, StdCall conventions, and limited marshalling scenarios. The payoff is a deployment artifact that looks like a traditional native DLL while still being authored in C#. In a week where VSTest is explicitly changing transitive dependencies (and last week emphasized keeping containers and servicing baselines current), Native AOT is another example of the same practical pressure: be explicit about what you ship, what you depend on, and what contracts you expose, because "it was there implicitly" is becoming less reliable over time.

- [High-Performance Distributed Caching with .NET and Postgres on Azure](https://devblogs.microsoft.com/dotnet/high-performance-distributed-caching-dotnet-postgres-azure/)
- [Using .NET Native AOT to build Windows WinAPI DLLs](https://weblog.west-wind.com/posts/2026/Apr/28/Using-NET-Native-AOT-to-build-Windows-WinAPI-DLLs)

### API surface and reliability: versioned OpenAPI and the outbox pattern, revisited

API versioning continues to be a practical concern for teams shipping public endpoints, and the .NET 10-focused guide on combining Asp.Versioning v10 with OpenAPI shows a clean pattern: generate one OpenAPI document per version and expose them as versioned endpoints like `/openapi/v*.json`. The post covers both controllers and Minimal APIs, then shows how to wire SwaggerUI or Scalar so consumers can browse the right contract without guessing which version a given schema belongs to. For teams trying to keep backward compatibility without freezing their API design, "one OpenAPI per version" tends to be easier to maintain than a single blended spec that grows conditional logic. It also ties back to last week''s reminder that frameworks and timelines move on (like the ASP.NET Core on .NET Framework end-of-support date): clear, versioned contracts make it easier to evolve APIs while you modernize runtimes and hosting models underneath.

Reliability in distributed systems showed up as well via On .NET Live, where Joao Antunes revisited the transactional outbox pattern using his OutboxKit toolkit. The discussion centers on the core trade-off the pattern is meant to address (atomicity between database state changes and message publishing), what tends to go wrong in real implementations, and alternative approaches when the operational cost of an outbox is too high for a given system.

- [Combining API versioning with OpenAPI in .NET 10 applications](https://devblogs.microsoft.com/dotnet/api-versioning-in-dotnet-10-applications/)
- [On .NET Live: Shaving the outbox pattern yak](https://www.youtube.com/watch?v=JoPY0yfElK4)

### Other .NET News

Visual Studio 2026 18.6 Insiders 3 now enables the TypeScript 7 Beta native preview by default, prioritizing compiler and language service performance while calling out feature gaps and known issues you will want to validate against your solution (especially if you rely on specific editor behaviors). The update also intersects with how teams manage project-local TypeScript via npm, since the "native language service preview" changes the default experience inside the IDE. It lands in the same broader "defaults are changing" bucket as the VSTest JSON dependency shift: IDE and tooling updates can quietly alter day-to-day behavior, so pinning versions and validating upgrades in CI matters.

- [TypeScript 7 Beta Now Enabled by Default in Visual Studio 2026 18.6 Insiders 3](https://devblogs.microsoft.com/visualstudio/typescript-7-beta-now-enabled-by-default-in-visual-studio-2026-18-6-insiders-3/)

Rick Strahl also revisited C# scripting and templating via Westwind.Scripting, explaining how the ScriptParser template engine works with a Handlebars-style syntax, Roslyn-based runtime compilation, caching, and newer layout/section support for file-based templates. If you maintain apps that generate text (emails, code, reports) and want a lightweight embedded templating approach without standing up a separate service, this is a useful tour of the architecture and trade-offs.

- [Revisiting C# Scripting with the Westwind.Scripting Templating Library, Part 1](https://weblog.west-wind.com/posts/2026/Apr/01/Revisiting-C-Scripting-with-the-WestwindScripting-Templating-Library-Part-1)',
    'This week in .NET was a mix of platform plumbing and practical building blocks: Microsoft pushed forward on modernizing the toolchain (especially inside Visual Studio), while several posts showed how .NET 10+ apps are increasingly composed from focused libraries for AI, caching, and API surface management. Coming right after last week''s split between "install the preview" (.NET 11 Preview 3) and "patch production now" (April 2026 servicing), the throughline is familiar: the platform keeps tightening defaults (dependencies, provenance, project systems), and teams need to validate those shifts early to avoid surprises later. At the same time, a couple of changes signaled where the ecosystem is heading next, including a notable test platform dependency shift that could surface as a breaking change in CI.',
    1777878000, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2026-05-04', 'TechHub',
    'TechHub', '6a6c0d38f89036ba4b9c87b9dd4e7ea0', ',GitHub Copilot,Usage Based Billing,AI Credits,Token Billing,Agent Workflows,Microsoft Foundry,GPT 5.5,Microsoft Agent Framework,A2A Protocol,MCP,Azure AI Foundry,Azure Monitor SLI/SLO,Microsoft Fabric OneLake,GitHub Actions Runners,Agent Governance,Roundups,AI,Azure,.NET,DevOps,ML,Security,',
    false, false, true,
    false, false,
    false, false, 4, '{"key_topics": ["GitHub Copilot", "Usage Based Billing", "AI Credits", "Token Billing", "Agent Workflows", "Microsoft Foundry", "GPT 5.5", "Microsoft Agent Framework", "A2A Protocol", "MCP", "Azure AI Foundry", "Azure Monitor SLI/SLO", "Microsoft Fabric OneLake", "GitHub Actions Runners", "Agent Governance"], "topic_type": "news", "roundup_summary": "Copilot moves toward more agentic workflows across IDEs and GitHub, while June 1 brings token-based billing, AI Credits, and new meters like Actions minutes for private-repo code review. In parallel, Microsoft and the broader ecosystem tightened the production story for agents with GPT-5.5 in Foundry, GA interoperability protocols (A2A and MCP), and more concrete guidance on observability, retrieval, and governance. Platform updates across Azure and Fabric focused on controlled operations: sovereign and disconnected deployments, least-privilege storage access, SLI/SLOs in Azure Monitor, and better real-time pipeline monitoring.", "roundup_relevance": "high"}'::jsonb
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2026-04-27
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2026-04-27', 'roundups', 'Agents Get Real: Tighter AI Limits, Stronger Tooling, Safer Ops',
    'This week in .NET was split between "ship-ready platform updates" and "what''s taking shape next." Ubuntu 26.04 landed with first-class .NET 10 support out of the box, while the .NET 11 wave continued to fill in long-requested language and tooling gaps (from discriminated unions to more practical scripting). On the app side, the teams kept pushing on real developer pain points: smoother Blazor list virtualization, clearer API docs when versioning is involved, and faster inner loops across containers and MAUI. That split mirrors last week''s pattern: alongside "you can try this now" items (like the Fabric ADO.NET preview driver and early .NET 11 Blazor validation direction), we are seeing more places where the platform is either ready to standardize (Linux baselines, container tags) or clearly signaling where core workflows are headed next (Blazor UX primitives, Aspire wiring, language features).

<!--excerpt_end-->

## .NET

### .NET 10 on Ubuntu 26.04 (and cleaner Linux + container setups)

Ubuntu 26.04 now treats .NET as a day-one citizen, with .NET 10 available immediately through apt-based installation, which simplifies provisioning for developers and reduces friction for production images built on the new LTS. The update also ties directly into container workflows: new `resolute` Docker image tags align with the Ubuntu 26.04 base, making it easier to standardize on a single distro version across dev, CI, and production. If you have apps pinned to older runtimes, the guidance calls out the `dotnet-backports` PPA as the supported path for installing .NET 8 and .NET 9 on 26.04 without fighting the default package feed.

The details matter for runtime behavior too. The post highlights Linux-specific considerations like cgroup v2 (relevant for container resource limits and how the runtime sizes thread pools and GC behavior) and calls out Native AOT as part of the broader story of making .NET deployments leaner and more predictable on Linux. Put together, Ubuntu 26.04 + .NET 10 is less about a single feature and more about having a well-lit path for "install, build, containerize, run" on an LTS baseline that many teams will standardize on for the next few years. It is also a useful backdrop for last week''s pipeline-oriented heads-up around PowerShell packaging changes (MSI to MSIX): between OS baselines, container images, and tooling distribution, small platform shifts tend to show up first in build agents and provisioning scripts, so this kind of "here is the supported path" guidance matters.

- [What’s new for .NET in Ubuntu 26.04](https://devblogs.microsoft.com/dotnet/whats-new-for-dotnet-in-ubuntu-2604/)

### Blazor: more flexible virtualization and early Aspire integration

Blazor''s UI performance story kept moving forward with a preview of upcoming .NET 11 improvements to the `Virtualize` component. The big practical change is support for variable-height items, which removes one of the most common sharp edges when trying to virtualize real-world lists (chat threads, feeds, cards, and anything where templates don''t render at a consistent height). Alongside that, new APIs for controlling scroll position aim to make virtualization feel less "hands off" and more like a component you can reliably integrate into rich UX patterns (restoring scroll on navigation, jumping to items, and building "infinite list" behaviors without hacks). Coming right after last week''s .NET 11 validation previews, this is another example of the team focusing on the parts of Blazor that routinely force custom plumbing (forms and lists) and moving those pain points into the framework''s default toolkit.

In parallel, the team showed in-progress Blazor integration work with .NET Aspire. The direction here is about making Blazor apps (both WebAssembly and Server) fit more naturally into Aspire''s model for configuration flow, service discovery, diagnostics, and scaling. The "Blazor gateway" concept hints at a more opinionated way to front Blazor applications in distributed setups, while deeper hooks into Aspire would mean fewer one-off scripts and less bespoke glue for wiring services together, observing them, and scaling them consistently across environments.

- [Blazor Community Standup: Blazor virtualization gets flexible](https://www.youtube.com/watch?v=Jd_a7uTuzmE)
- [Blazor Community Standup | Blazor integration with Aspire](https://www.youtube.com/watch?v=7ycBQh1Ilag)

### .NET 11 previews: C# discriminated unions and more complete scripting

C# 15''s discriminated unions were the headline language feature discussed this month, and the coverage this week focused on what actually shipped (merged for .NET 11 preview) and what to be careful about before betting designs on it. The promise is familiar if you''ve used F# or languages with first-class sum types: express domain states explicitly, push invalid states out of your model, and let the compiler help you handle every case. The caveat is that preview implementations often come with constraints that affect ergonomics, codegen, and interop patterns, so the advice here is to treat the feature as "learn it and experiment" rather than "rewrite core models today."

On the scripting side, .NET 11 Preview 3 filled a long-standing gap by adding file references so scripts can be structured across multiple files. That moves scripting closer to something you can maintain over time: shared helpers, reusable modules, and scripts that do not collapse into a single growing `.csx` file. For teams using scripts for build glue, migrations, or one-off automation, this is the difference between "handy demo" and "tool you can keep in a repo." It also lines up with last week''s theme of making .NET fit existing, familiar workflows (like exposing Fabric Spark through ADO.NET patterns): both changes reduce the "special case" tooling teams have to maintain by letting them use more standard structures and conventions.

- [Discriminated Unions Are Finally in .NET But](https://www.youtube.com/watch?v=C5mozkE5x20)
- [.NET Scripting is Finally Complete](https://www.youtube.com/watch?v=VLXXXYQ8SEA)

### Visual Studio and VS Code workflows: better navigation and repeatable dev environments

Visual Studio got a bookmarking rethink with Bookmark Studio, an experimental extension that turns bookmarks into something you can actually manage at scale. Instead of a flat, fragile set of markers, it adds slot-based keyboard navigation, a Bookmark Manager tool window, optional organization, and export options (text/Markdown/CSV) so bookmarks can leave the IDE when you need to share context or track hotspots. A key quality-of-life detail is improved tracking as code changes, which matters because bookmarks that drift or disappear after refactors tend to get abandoned. After last week''s Visual Studio "floating windows" ergonomics tip, this continues the theme of shaving down everyday IDE friction, especially for developers working across multiple files and screens where navigation overhead adds up.

On the VS Code side, a practical Dev Containers walkthrough focused on a setup many teams actually want: a .NET 8 Web API with PostgreSQL using Docker Compose, ready to run inside a containerized dev environment. The guide leans into the reality that beginners hit: Docker Desktop vs Docker Engine differences, WSL2 integration quirks, and the kinds of "it should work but doesn''t" issues that slow teams down when onboarding. The value here is less about the happy path and more about having a troubleshooting checklist that makes Dev Containers viable as a default workflow instead of an occasional experiment. Paired with this week''s Ubuntu 26.04 + .NET 10 baseline story, it reinforces a consistent thread: standardizing environments (Linux versions, container tags, devcontainer configs) is increasingly the practical path to fewer "works on my machine" surprises.

- [Bookmark Studio: evolving bookmarks in Visual Studio](https://devblogs.microsoft.com/visualstudio/bookmark-studio-evolving-bookmarks-in-visual-studio/)
- [Dev Containers for .NET in VS Code: A Beginner‑Friendly Guide That Actually Works](https://techcommunity.microsoft.com/t5/microsoft-developer-community/dev-containers-for-net-in-vs-code-a-beginner-friendly-guide-that/ba-p/4507785)

### ASP.NET Core APIs: aligning versioning with OpenAPI output

API versioning and OpenAPI often collide in subtle ways: your routing and behavior are version-aware, but your generated OpenAPI documents can end up duplicated, inconsistent, or require a lot of manual configuration to keep tidy. This week''s ASP.NET Community Standup dug into how ASP.NET API Versioning fits with the newer ASP.NET Core OpenAPI packages in .NET 10, with an emphasis on generating aligned OpenAPI documents for both Minimal APIs and controllers without repeating configuration in multiple places.

The practical takeaway is that the ecosystem is converging on a cleaner approach: treat versioning as a first-class input into document generation so each version gets the right surface area and metadata, while keeping the setup maintainable as the number of endpoints grows. For teams publishing public APIs (or even internal APIs consumed by multiple services), this is the kind of plumbing work that reduces churn when you add a v2, keep v1 alive, and still want accurate client generation and interactive docs.

- [ASP.NET Community Standup: Combining API Versioning with OpenAPI](https://www.youtube.com/watch?v=7m3r6slW68U)

### Other .NET News

Rick Strahl shared a deeper look at using Westwind.Scripting''s `ScriptParser` in a real production workflow (Documentation Monster), covering practical templating concerns like layout/content composition, base URL fixups for preview vs published output, and error handling that distinguishes compilation failures from runtime failures. It is a nice real-world complement to the ".NET scripting is finally complete" thread this week: as the core scripting story gets more maintainable (multi-file structure), the community examples keep showing what "scripts you can keep in a repo" actually look like in production.

- [Putting the Westwind.Scripting C# Templating Library to work, Part 2](https://weblog.west-wind.com/posts/2026/Apr/23/Putting-the-WestwindScripting-Templating-Library-to-work-Part-2)

The .NET MAUI team livestreamed a tight inner-loop workflow using MauiDevFlow, where AI agents can inspect a running app (visual tree, screenshots, logs) and drive build-deploy-inspect-fix iterations from the terminal, including scenarios that involve Blazor WebView and Chrome DevTools Protocol (CDP) style inspection. Coming after last week''s mix of "trial it now" previews and pipeline-impacting changes, this fits the same practical arc: reduce the amount of manual, local-only setup work required to iterate quickly, and make repeatable workflows (whether in containers, on Linux LTS images, or in app tooling) the default.

- [.NET MAUI Engineering Team Live Stream: AI-Powered .NET MAUI Development with MauiDevFlow](https://www.youtube.com/watch?v=vhrpjCJw1CY)',
    'This week in .NET was split between "ship-ready platform updates" and "what''s taking shape next." Ubuntu 26.04 landed with first-class .NET 10 support out of the box, while the .NET 11 wave continued to fill in long-requested language and tooling gaps (from discriminated unions to more practical scripting). On the app side, the teams kept pushing on real developer pain points: smoother Blazor list virtualization, clearer API docs when versioning is involved, and faster inner loops across containers and MAUI. That split mirrors last week''s pattern: alongside "you can try this now" items (like the Fabric ADO.NET preview driver and early .NET 11 Blazor validation direction), we are seeing more places where the platform is either ready to standardize (Linux baselines, container tags) or clearly signaling where core workflows are headed next (Blazor UX primitives, Aspire wiring, language features).',
    1777273200, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2026-04-27', 'TechHub',
    'TechHub', 'c141d2fa049b460019f6a3bb085b13a4', ',GitHub Copilot,GPT 5.5,Copilot CLI,VS Code Agents,MCP,Azure AI Foundry,Microsoft Agent Framework,Microsoft Fabric,MLflow,Azure DevOps,GitHub Actions,CodeQL,Private Link,Managed Identity,Microsoft Defender XDR,Roundups,AI,Azure,.NET,DevOps,ML,Security,',
    false, false, true,
    false, false,
    false, false, 4, '{"key_topics": ["GitHub Copilot", "GPT 5.5", "Copilot CLI", "VS Code Agents", "MCP", "Azure AI Foundry", "Microsoft Agent Framework", "Microsoft Fabric", "MLflow", "Azure DevOps", "GitHub Actions", "CodeQL", "Private Link", "Managed Identity", "Microsoft Defender XDR"], "topic_type": "news", "roundup_summary": "This week pushed AI assistants further into real workflows (IDE agents, azd deployments, and MCP-connected tools) while tightening the controls that keep costs and governance predictable, including Copilot individual plan limits and admin-gated access to GPT-5.5. Across Azure and Fabric, the focus stayed on secure-by-default operations (private networking, managed identities, outbound controls) and practical platform plumbing for MLOps, streaming, and telemetry. DevOps and security updates added more change-management work (TLS SHA-1 removal, longer GitHub App tokens), plus concrete improvements in scanning, dependency visibility, and Defender-guided incident disruption.", "roundup_relevance": "high"}'::jsonb
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2026-04-20
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', 'Operational AI Agents: PR Workflows, Governance Controls, and Tooling Baselines',
    'This week''s .NET updates split between moving forward and staying current. .NET 11 Preview 3 shipped runtime/SDK/library/framework updates aimed at everyday development, while April 2026 servicing releases delivered security fixes across supported .NET and .NET Framework versions. Building on last week''s .NET 11 direction-setting items (like Blazor validation previews), this is another preview step you can install and test, alongside reminders to keep production stacks patched. Microsoft also set a deadline for an "ASP.NET Core on .NET Framework" escape hatch, pushing teams toward modern .NET for web workloads.

<!--excerpt_end-->

## .NET

### .NET 11 Preview 3: runtime, SDK/CLI, web/WASM, MAUI, EF Core, and containers

.NET 11 Preview 3 updates the runtime, SDK/CLI, BCL, C#, ASP.NET Core, .NET MAUI, EF Core, and official container images, with emphasis on performance and faster inner-loop iteration. Following last week''s focus on evolving app workflows (for example, Blazor validation), Preview 3 spreads incremental improvements across the stack.

Runtime/JIT changes include optimizations for switch statements, bounds checks, and casts to reduce hot-path overhead without code changes. It also makes "runtime async" available without preview-API opt-in, which reduces friction for teams evaluating async runtime capabilities during previews.

BCL updates mix control and safety. System.Text.Json adds controls for naming and ignore-default handling to reduce the need for custom converters. Compression expands: Zstandard (zstd) support moves into System.IO.Compression, and ZIP reading adds CRC32 validation to surface corrupt archives earlier. Lower-level IO improvements expand pipe support via SafeFileHandle and RandomAccess for infra and interop-heavy code. One behavior change is that Regex now recognizes all Unicode newline sequences, which can change matches on inputs that contain non-ASCII newlines (relevant for cross-platform parsing).

SDK/CLI improvements target large repos and fast iteration. You can edit solution filters from the CLI, which helps in monorepos and focused builds. File-based apps can span multiple files, making script-like prototypes easier to organize. `dotnet run` adds `-e` for setting environment variables directly. `dotnet watch` adds Aspire support, crash recovery, and Windows desktop improvements, aiming for more resilient hot reload and watch workflows across cloud-native and desktop apps. This pairs with last week''s pipeline and machine operational notes: teams will notice these changes in local iteration and automation.

For web and browser, ASP.NET Core adds Zstandard response compression and request decompression as an alternative to gzip/brotli. Blazor `Virtualize` can adapt to variable-height items at runtime, reducing jank when item sizes vary and continuing last week''s thread of making common UI patterns require less custom code. HTTP/3 processing starts earlier in the pipeline to reduce latency and overhead. Browser/WASM updates add WebCIL support and debugging improvements, affecting packaging and developer experience for Blazor WebAssembly and other .NET-in-browser workloads.

C# previews experimental union types support for discriminated-union-style modeling. .NET MAUI updates include Maps improvements (clustering, styling, richer APIs), XAML/styling tweaks for startup and iteration, and a built-in `LongPressGestureRecognizer`. .NET for Android adds Android 17 / API 37 preview support to validate upcoming platform changes.

EF Core adds performance and configuration controls. `ChangeTracker.GetEntriesForState()` avoids extra change detection when inspecting tracked state. DbContext config can remove providers and add pooled factories for more flexible DI/provider/pooling combinations. Migrations get more control and clearer feedback, and query generation removes unnecessary joins in some cases. SQL Server provider support adds JSON APIs for teams using JSON-centric schema patterns.

Supply-chain hardening shows up in containers: official .NET container images are now signed, improving provenance for CI/CD policies that require signatures. Read alongside last week''s PowerShell installer transition warning, it is another reminder that build and deploy inputs (base images, installers, agent tooling) matter as much as application code. Preview 3 guidance points to installing the .NET 11 SDK Preview 3 and using Visual Studio 2026 Insiders on Windows, or VS Code with C# Dev Kit.

- [.NET 11 Preview 3 is now available!](https://devblogs.microsoft.com/dotnet/dotnet-11-preview-3/)

### Servicing and support timelines: April 2026 patches and a deadline for ASP.NET Core 2.3 on .NET Framework

Microsoft shipped April 14, 2026 servicing updates for supported .NET and .NET Framework versions, covering security and non-security fixes with links to release notes, installers/binaries, MCR container images, Linux package guidance, and known issues. After last week''s mix of preview features and pipeline-affecting policy shifts, this reinforces the "do not fall behind" track: servicing updates are the immediate production task, and previews are for what comes next.

Patched .NET releases are .NET 10.0.6, .NET 9.0.15, and .NET 8.0.26, with pointers to GitHub release notes and milestone/changelog queries for ASP.NET Core, EF Core, the runtime, and WinForms. For teams running multiple app types, those links help you audit changes in the parts you ship.

Security-wise, the post lists multiple CVEs across .NET and .NET Framework, including denial of service, security feature bypass, and remote code execution (affecting combinations of .NET 10/9/8 and multiple .NET Framework versions). Practically, patching means more than updating dev SDKs: rebuild and redeploy containers on updated MCR base images, update build agents, and pull forward Linux package installs, while checking known issues before broad rollout.

Microsoft also set an end-of-support date for ASP.NET Core 2.3 on .NET Framework: April 7, 2027. After that, it gets no security patches, bug fixes, or support, creating a deadline for orgs that adopted ASP.NET Core but stayed on .NET Framework. The recommended path is modern ASP.NET on .NET 10, aligning with ongoing runtime and framework improvements and enabling cross-platform hosting. Context matters: ASP.NET Core 3.0 dropped .NET Framework support in 2019, and ASP.NET Core 2.3 (early 2025) was a servicing-oriented baseline for Framework users, effectively a re-release of 2.1. In some cases, 2.1 -> 2.3 behaved more like a compatibility tradeoff than a simple bump. With install stats showing these packages still widely used, the EoS date is likely to surface in backlogs soon, especially for stable Windows/IIS apps on long timelines.

- [.NET and .NET Framework April 2026 servicing releases updates](https://devblogs.microsoft.com/dotnet/dotnet-and-dotnet-framework-april-2026-servicing-updates/)
- [Microsoft calls time on ASP.NET Core 2.3 on .NET Framework](https://www.devclass.com/devops/2026/04/13/microsoft-calls-time-on-aspnet-core-23-on-net-framework/5216962)',
    'This week''s .NET updates split between moving forward and staying current. .NET 11 Preview 3 shipped runtime/SDK/library/framework updates aimed at everyday development, while April 2026 servicing releases delivered security fixes across supported .NET and .NET Framework versions. Building on last week''s .NET 11 direction-setting items (like Blazor validation previews), this is another preview step you can install and test, alongside reminders to keep production stacks patched. Microsoft also set a deadline for an "ASP.NET Core on .NET Framework" escape hatch, pushing teams toward modern .NET for web workloads.',
    1776675600, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2026-04-20', 'TechHub',
    'TechHub', '495349cbac5b3d4e84eafee6b7c8f5ce', ',GitHub Copilot,Copilot Cloud Agent,Copilot CLI,Custom Skills,Model Routing,Data Residency,FedRAMP,MCP,Microsoft Foundry,Agent Governance,AKS,Kubernetes Gateway API,Microsoft Fabric,OneLake,.NET 11,AI,Azure,.NET,DevOps,ML,Security,Roundups,',
    false, false, true,
    false, false,
    false, false, 4, NULL
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2026-04-13
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', 'Agents Across Copilot: Controls, MCP Tooling, and Safer Operations Loops',
    'This week''s .NET updates focused on practical changes: a new way to run Spark SQL from ADO.NET code, early direction on Blazor validation in .NET 11, and a Windows packaging change for PowerShell that will affect machines and build agents. Compared to last week''s "what''s next" previews, this week is more "here is what you can trial now," plus a policy shift that can impact pipelines.

<!--excerpt_end-->

## .NET

### .NET data access to Microsoft Fabric Spark (ADO.NET preview driver)

Microsoft released a preview ADO.NET provider for Microsoft Fabric Data Engineering, letting .NET apps connect to Fabric and run Spark SQL using standard ADO.NET patterns instead of custom HTTP calls to Livy. It uses Fabric''s Livy APIs but exposes familiar abstractions (`DbConnection`, `DbCommand`, `DbDataReader`, `DbParameter`, `DbProviderFactory`) so existing ADO.NET-shaped codebases can adapt with less refactoring. It supports typical command/reader flows and parameterized queries, which makes it easier to integrate Spark SQL execution into existing repository layers or ETL/ELT services without maintaining a separate client stack.

The driver supports Microsoft Entra ID across common auth flows: Azure CLI auth, interactive browser login, client credentials, certificate auth, and direct access-token usage. It also targets efficiency and resilience with connection pooling and Spark session reuse, async prefetch for large result sets, and auto-reconnect to recover sessions after failures. It claims broad Spark SQL type coverage, including `ARRAY`, `MAP`, and `STRUCT`, which matters for lakehouse-shaped data in .NET pipelines. Overall, it gives .NET teams a more standard way to query and manage Fabric Spark from services and tools, aligning with the approach of fitting existing .NET idioms rather than requiring a new programming model.

- [Microsoft ADO.NET Driver for Microsoft Fabric Data Engineering (Preview)](https://blog.fabric.microsoft.com/en-US/blog/microsoft-ado-net-driver-for-microsoft-fabric-data-engineering-preview/)

### Blazor in .NET 11: validation model previews (async rules, localization, render-mode flexibility)

In the Blazor Community Standup, the team previewed .NET 11 validation improvements intended to reduce custom plumbing around `EditForm`. Like last week''s .NET 11 Preview 2 direction-setting items, this is another look at where core workflows are headed so teams can rely less on homegrown patterns.

A key focus is built-in asynchronous validation so forms can validate against async-backed rules (username availability, DB checks, external lookups) without pushing developers into custom "validate on submit" flows outside Blazor''s normal validation model.

They also previewed localized validation messages to reduce manual resource mapping and custom error pipelines in multi-language apps. Finally, they discussed enabling client-side validation without requiring interactive render mode, which helps when teams mix render modes and do not want interactivity only to get validation feedback. Together, the direction is a more flexible validation stack across async rules, localized UX, and modern render-mode composition. It mirrors last week''s theme of "stable defaults with a lane for evolution," applied to everyday app UX.

- [''Blazor Community Standup: Blazor validation in .NET 11''](https://www.youtube.com/watch?v=X-qAr4uqAzc)

### Other .NET News

PowerShell packaging on Windows is changing starting with PowerShell 7.7-preview.1 (April 2026): MSIX becomes the primary installer, and new releases will no longer ship an MSI (MSI remains for existing releases like 7.6). Teams that automate PowerShell rollout across dev machines, CI runners, and managed endpoints will need to shift MSI-based deployment/upgrade pipelines to MSIX tooling for future versions. Microsoft cites a more predictable servicing model (updates, differential updates) and accessibility requirements, while noting gaps still being addressed, especially around remoting and system-level execution (Task Scheduler, services). Practically, it is an early warning to review and update rollout pipelines.

- [PowerShell MSI package deprecation and preview updates](https://devblogs.microsoft.com/powershell/powershell-msi-deprecation/)

Visual Studio shared a workflow tip for multi-monitor users: a setting controls whether floating windows are "owned" by the main IDE window (Tools > Options > Environment > Windows > Floating Windows). Setting ownership to "None" can make floating windows behave more like top-level windows (separate taskbar buttons, fewer always-on-top quirks, and staying visible when the IDE is minimized), which pairs well with PowerToys FancyZones. It also notes other modes ("Tool Windows" and "Documents and Tool Windows") and a shortcut: Ctrl + double-click a tool window title bar to toggle dock/float.

- [Take full control of your floating windows in Visual Studio](https://devblogs.microsoft.com/visualstudio/take-full-control-of-your-floating-windows-in-visual-studio/)',
    'This week''s .NET updates focused on practical changes: a new way to run Spark SQL from ADO.NET code, early direction on Blazor validation in .NET 11, and a Windows packaging change for PowerShell that will affect machines and build agents. Compared to last week''s "what''s next" previews, this week is more "here is what you can trial now," plus a policy shift that can impact pipelines.',
    1776070800, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2026-04-13', 'TechHub',
    'TechHub', '29abf1fc55e6c4d43f1cdff8cf34f5f4', ',GitHub Copilot,Copilot CLI,VS Code Agents,Copilot Cloud Agent,MCP,BYOK,Local Models,Azure AI Foundry,Foundry Local,Microsoft Entra ID,Managed Identity,OpenTelemetry,Application Insights,GitHub Actions,DevSecOps,AI,Azure,.NET,DevOps,ML,Security,Roundups,',
    false, false, true,
    false, false,
    false, false, 4, NULL
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2026-04-06
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'Agents as Everyday Teammates, With Practical Guardrails: Copilot, Azure, and Secure Ops',
    'This week’s .NET items leaned toward "what’s next," with early looks at language features and framework experiments that could change how you model APIs and configure apps. MAUI also clarified how to try new UI ideas without waiting for full releases. This split between stable baselines and previews/experiments continues from last week: alongside GA paths like Aspire on App Service, the .NET 11 Preview 2 thread keeps producing deeper language/runtime experiments, and MAUI is formalizing an "expect churn" lane through an experiments hub.

<!--excerpt_end-->

## .NET

### C# 15 union types in .NET 11 Preview

C# 15 (starting with .NET 11 Preview 2) introduces union types as a first-class way to define a closed set of value shapes without `object`, marker interfaces, or awkward base-class hierarchies. Following last week’s .NET 11 Preview 2 coverage, this is another Preview 2 feature that is likely to evolve as tooling and runtime pieces land. With `union`, you can declare `public union Pet(Cat, Dog, Bird);`, and the compiler treats the cases as complete: it supports implicit conversions from each case type (for example, `Pet pet = new Dog("Rex");`) and enforces exhaustive pattern matching in `switch` expressions. The maintenance benefit is clear: if you add a new case later, existing switches can warn when they are no longer exhaustive.

The preview includes important semantics and caveats. Patterns generally apply to the generated `Value` (auto-unwrapping), except `var` and `_`, which bind/match the union itself. Nullability matters: the default union value has `Value == null`, and the `null` pattern checks whether `Value` is null; if any cases are nullable (for example, `Bird?`), you must handle `null` explicitly for exhaustiveness. Under the hood, `union` is shorthand for a compiler-generated struct with per-case constructors and a `Value` of type `object?`, so value-type cases box by default.

For library authors and performance-sensitive code, "custom union types" are also supported. If you annotate a type with `[System.Runtime.CompilerServices.Union]` and follow the expected shape (public single-parameter constructors plus a public `Value` property), the compiler treats it as a union. Adding `HasValue` / `TryGetValue` can enable union-aware patterns that avoid boxing for value-type cases. To try it now, install the .NET 11 Preview SDK, target `net11.0`, set `<LangVersion>preview</LangVersion>`, and add runtime polyfills for `UnionAttribute` and `IUnion` (not included in .NET 11 Preview 2 yet). IDE support is expected via upcoming Visual Studio Insiders builds and is already in the latest C# Dev Kit Insiders.

- [Explore union types in C# 15](https://devblogs.microsoft.com/dotnet/csharp-15-union-types/)

### Contextual options: per-request configuration via an experimental extensions package

The options pattern got an experimental add-on: `Microsoft.Extensions.Options.Contextual`, a NuGet package that adds a contextual layer on top of `IOptions<T>`. Building on last week’s theme of code-first workflows across more environments, it keeps configuration DI-driven while letting it adapt to request/tenant/user context. Instead of global or named options, you resolve `IContextualOptions<TOptions, TContext>` and call `GetAsync(context)` to compute options for a specific context. The walkthrough uses an ASP.NET Core "weather forecast" app with an `AppContext` (annotated `[OptionsContext]` and `partial`) carrying fields like `UserId` and `Country`, then derives defaults from context at the call site.

Mechanically, there are three parts: a source generator (`ContextualOptionsGenerator`) emits an `IOptionsContext` implementation; you implement an `IOptionsContextReceiver` that consumes key/value pairs via `Receive<T>(string key, T value)`; and you register an additional contextual configure callback `(IOptionsContext ctx, TOptions opts)` to apply derived values. The post calls out a maintainability risk: receivers are coupled to context properties via string keys (property names), so renames can silently change behavior. There is also the cost of adopting an `[Experimental]` API: the package triggers `EXTEXP0018` unless you opt in, and generated code is also experimental, so teams often suppress warnings broadly (for example, `<NoWarn>$(NoWarn);EXTEXP0018</NoWarn>`). The conclusion is to evaluate it mainly if you already rely on `IOptions` and need true per-context config that named options cannot express; otherwise feature flag tooling (`Microsoft.FeatureManagement`, OpenFeature) may fit better.

- [Configuring contextual options with Microsoft.Extensions.Options.Contextual](https://andrewlock.net/configuring-contextual-options-with-microsoft-extensions-options-contextual/)

### .NET MAUI’s new home for experiments: maui-labs

The .NET MAUI Community Standup introduced maui-labs as the official home for experimental and community-driven MAUI work, including prototypes and in-progress ideas not ready for stable MAUI. This fits the pattern we have been tracking: last week’s MAUI + Avalonia backend preview (Linux and WebAssembly) showed active experimentation around rendering and reach, and maui-labs clarifies where that work should live so teams can try it without confusing it with supported MAUI. The practical benefit is clearer boundaries: developers get one place to follow and test early work on expanded platform support, alternate rendering options, and exploratory features, with a clearer path for what might later graduate into the product.

- [''.NET MAUI Community Standup: Introducing maui-labs''](https://www.youtube.com/watch?v=IfCIubKbyqw)',
    'This week’s .NET items leaned toward "what’s next," with early looks at language features and framework experiments that could change how you model APIs and configure apps. MAUI also clarified how to try new UI ideas without waiting for full releases. This split between stable baselines and previews/experiments continues from last week: alongside GA paths like Aspire on App Service, the .NET 11 Preview 2 thread keeps producing deeper language/runtime experiments, and MAUI is formalizing an "expect churn" lane through an experiments hub.',
    1775466000, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2026-04-06', 'TechHub',
    'TechHub', '7e86e52b30822bee8ade741a9581ce8a', ',GitHub Copilot,Copilot Cloud Agent,Copilot CLI,Copilot SDK,MCP,VS,VS Code,Azure AI Foundry,Microsoft Agent Framework,Copilot Studio,Microsoft Fabric,GitHub Actions,Supply Chain Security,CodeQL,Azure Service Bus,AI,Azure,.NET,DevOps,ML,Security,Roundups,',
    false, false, true,
    false, false,
    false, false, 4, NULL
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2026-03-30
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', 'Agents in the Workflow: PR Automation, Governed AI, and Safer CI/CD',
    'This week''s .NET updates focused on meeting developers where they are: keeping code-first workflows while widening where apps can run. It continues last week''s two-track theme: maintain a stable production baseline (PowerShell 7.6 LTS on .NET 10, smoother VS Code Insiders tooling) while trying newer .NET 11 Preview 2 features (like MAUI Maps pin clustering). This week, that split shows up as Aspire getting a supported Azure deployment path and MAUI exploring broader targets via Avalonia''s rendering stack.

<!--excerpt_end-->

## .NET

### .NET Aspire on Azure App Service (GA)

Aspire on Azure App Service reached GA, taking Aspire from local orchestration to a deployable, AppHost-defined topology on App Service without abandoning the Aspire model. Like last week''s focus on maintaining a known-good baseline, this GA gives teams already using AppHost locally a clearer production path with fewer conceptual jumps.

The core is the `Aspire.Hosting.Azure.AppService` NuGet package. You define your App Service environment in AppHost code alongside familiar local constructs: project references, health checks (for example, `WithHttpHealthCheck("/health")`), and endpoint exposure (`WithExternalHttpEndpoints()`). For teams standardizing on code-defined configuration, keeping the app''s "shape" in AppHost reduces drift between local and hosted environments.

A practical GA detail is support for Deployment Slots, mapping to typical staging/production workflows. From AppHost you can declare a slot (for example, `.WithDeploymentSlot("dev")`) with behavior to note: if production does not exist, deployment creates production and the staging slot with identical config; if production exists, deployment targets only the staging slot. Scaling is supported manually (via AppHost code or portal), and rule-based scaling is available through Azure Monitor Autoscale, but fully automatic scaling is not part of Aspire-on-App-Service yet. Overall, this is a bridge from local composition to managed App Service while keeping AppHost as the source of truth for dependencies and topology.

- [Aspire on Azure App Service is now Generally Available](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/aspire-on-azure-app-service-is-now-generally-available/ba-p/4505549)

### .NET MAUI + AvaloniaUI backend (preview) for Linux and WebAssembly

Last week''s .NET 11 Preview 2 notes highlighted MAUI control work (like Maps pin clustering). This week''s MAUI update is about platform reach: a preview AvaloniaUI "backend" for .NET MAUI adds Linux desktop and WebAssembly targets. It''s based on .NET 11 preview and is expected to remain preview until .NET 11 GA (projected around November), matching the "try it, expect churn" posture implied by last week''s preview coverage.

The approach swaps rendering. Instead of only MAUI''s native handler model, MAUI apps can render UI using Avalonia''s custom-drawn control stack, either alongside or replacing MAUI controls. The trade is clear: keep the MAUI app model while gaining Avalonia''s cross-platform reach (Windows, macOS, Linux, iOS, Android, WebAssembly) without rewriting into an Avalonia-first architecture. It also creates a continuation path: the same MAUI app can use new MAUI controls on mobile while experimenting with Linux/browser targets via the backend, though behavior/rendering may differ because the control stack changes.

The preview has constraints teams need to plan for. Microsoft MAUI API coverage is incomplete, especially around storage/media, so workarounds or waiting may be required. Linux relies on X11/XWayland rather than Wayland, which can block modern desktops. On Windows, it does not yet support hosting Avalonia controls inside WinUI (MAUI''s Windows stack), limiting hybrid composition. Engineering from this backend is also feeding into Avalonia 12 controls/APIs and efforts to reduce MAUI/Avalonia control differences. For MAUI teams that need Linux desktop or browser targets, it''s a workable option if you can tolerate preview gaps and cross-platform UI churn.

- [Avalonia adds Linux and WebAssembly targets to .NET MAUI (preview, .NET 11)](https://www.devclass.com/development/2026/03/24/avaloniaui-enhances-net-maui-with-linux-and-webassembly-support/5209515)',
    'This week''s .NET updates focused on meeting developers where they are: keeping code-first workflows while widening where apps can run. It continues last week''s two-track theme: maintain a stable production baseline (PowerShell 7.6 LTS on .NET 10, smoother VS Code Insiders tooling) while trying newer .NET 11 Preview 2 features (like MAUI Maps pin clustering). This week, that split shows up as Aspire getting a supported Azure deployment path and MAUI exploring broader targets via Avalonia''s rendering stack.',
    1774861200, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2026-03-30', 'TechHub',
    'TechHub', '89652a625ddbf0972ef8523bcd0ef0ba', ',GitHub Copilot,AI Agents,Pull Requests,GitHub Actions,MCP,Azure AI Foundry,Foundry Local,Microsoft Fabric,Data Factory,Dataflow Gen2,Dbt,AKS,Private Link,Supply Chain Security,CodeQL,AI,Azure,.NET,DevOps,ML,Security,Roundups,',
    false, false, true,
    false, false,
    false, false, 4, NULL
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2026-03-23
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', 'Operating AI Agents Like Software: Governance, Tools, and Operational Loops',
    'This week''s .NET updates landed where teams feel everyday friction: shells and editors that drive automation and debugging, and UI controls that need to stay responsive under real data. Building on last week''s "apply updates" focus (.NET servicing guidance and the macOS VS Code debugger hotfix) plus "try new features" (.NET 11 Preview 2 wave), this week continues the same two-track story: a clearer production baseline for tooling and a Preview 2 feature that is easy to validate in apps.

<!--excerpt_end-->

## .NET

### PowerShell 7.6 LTS on .NET 10: production automation baseline, with shell and module refinements

PowerShell 7.6 is now GA as the next LTS release, built on .NET 10 (LTS), giving teams a stable target for production scripting and automation where predictable behavior matters. Like last week''s servicing guidance ("keep runtime/tooling patched," including the out-of-band .NET 10.0.5 VS Code macOS debugger fix), 7.6 provides a steadier baseline for build agents, automation runners, and operator scripts, especially for fleets standardizing on .NET 10 LTS.

Alongside engine reliability and cross-platform consistency work, the release updates key in-box modules (PSReadLine, PSResourceGet, ThreadJob), so environments may see behavior changes simply by moving to 7.6 even if modules were not intentionally updated before. This mirrors last week''s "hidden dependency" point: module and runtime layers are often where friction appears first, including on build agents and base images.

Much of the polish is focused on interactive authoring and discoverability. Tab completion has many fixes and expansions (paths across providers, parameter value completions, more contexts where completion works, module completion by shortname), which reduces manual lookup and trial-and-error. Automation-visible updates include `Get-Clipboard -Delimiter`, `Register-ArgumentCompleter -NativeFallback` for native-command completion, `Get-Command -ExcludeModule`, `New-Item` treating `-Target` literally, and `Start-Process -Wait` improving polling efficiency to reduce overhead in scripts waiting on child processes.

On platform conventions, 7.6 adds aliases `PSForEach()` and `PSWhere()` for intrinsic `ForEach()` and `Where()` methods and respects `NO_COLOR` for stderr output in the console host. API and compatibility updates include Unix SystemPolicy public APIs being visible but no-op for `PowerShellStandard.Library`, and certificate DNS name handling using `X509SubjectAlternativeNameExtension.EnumerateDnsNames()`.

Several formerly optional or preview features are now treated as mainstream (`PSFeedbackProvider`, `PSNativeWindowsTildeExpansion`, `PSRedirectToVariable`, `PSSubsystemPluginModel`), which reduces "feature flag semantics" surprises in production. As with any LTS bump, there are breaking changes to validate: `Join-Path` changes `-ChildPath` to `string[]`, `WildcardPattern.Escape()` now escapes lone backticks correctly (changing outputs for scripts relying on prior behavior), and the `GetHelpCommand` trace source name drops a trailing space (affecting trace-name matching). The post links install guidance and What''s New docs and notes ongoing PowerShell 7.7 previews.

- [Announcing PowerShell 7.6 (LTS) GA Release](https://devblogs.microsoft.com/powershell/announcing-powershell-7-6/)

### .NET MAUI 11 Preview 2: built-in map pin clustering for Android and iOS/Mac Catalyst

Following last week''s .NET 11 Preview 2 rundown (including the MAUI Map control landing in Preview 2), this is one of the most visible additions in apps: .NET MAUI 11 Preview 2 adds built-in pin clustering in the `Map` control for Android and iOS/Mac Catalyst. With `IsClusteringEnabled="True"`, overlapping pins collapse into cluster markers with a count when zoomed out, then expand as users zoom in, which improves usability and performance on crowded maps.

The feature supports practical organization via `ClusteringIdentifier` strings per `Pin`: pins with the same identifier cluster together (for example, "coffee"), while different identifiers will not merge even if nearby (keeping "parks" separate). For interaction, `Map` exposes `ClusterClicked`; `ClusterClickedEventArgs` includes the cluster `Pins`, `Location`, and a `Handled` flag to suppress default zoom-to-cluster behavior (the sample shows listing pin labels via `DisplayAlert`).

Under the hood, Android uses a custom grid-based algorithm recalculated on zoom changes and avoids external dependencies, while iOS/Mac Catalyst uses MapKit''s native `MKClusterAnnotation` for platform-native behavior and animations. To try it you need .NET 11 Preview 2 and updated MAUI workloads; the post also calls out Visual Studio 2026 Insiders on Windows or VS Code with C# Dev Kit, continuing last week''s "validate previews in real toolchains" guidance. Samples are updated (the `maui-samples` Maps demo adds a Clustering page) and Microsoft Learn docs are refreshed.

- [Pin Clustering in .NET MAUI Maps](https://devblogs.microsoft.com/dotnet/pin-clustering-in-dotnet-maui-maps/)

### Other .NET News

Last week''s .NET servicing roundup included the out-of-band macOS VS Code debugger fix, and this week''s VS Code Insiders notes continue the "make the daily loop smoother" direction for cross-platform debugging and local workflows.

VS Code Insiders kept refining cross-platform workflows that .NET teams tend to hit quickly. The integrated browser can now bypass certificate errors for localhost HTTPS with self-signed certs, which helps local secure-context loops (OAuth redirects, secure cookies, service workers) without switching browsers or reworking trust stores. Debug config sharing is cleaner with new `launch.json` top-level `"windows"`, `"linux"`, `"osx"` properties so repos can keep platform-specific entries in one file while hiding irrelevant configs on other OSes, reducing noise in the Run and Debug UI. There''s also a WSL fix for "Reveal in File Explorer," plus UX/accessibility improvements like better screen reader labels and image carousel zoom for inspecting assets.

- [''Visual Studio Code 1.113 (Insiders): update notes for March 2026''](https://code.visualstudio.com/updates/v1_113)',
    'This week''s .NET updates landed where teams feel everyday friction: shells and editors that drive automation and debugging, and UI controls that need to stay responsive under real data. Building on last week''s "apply updates" focus (.NET servicing guidance and the macOS VS Code debugger hotfix) plus "try new features" (.NET 11 Preview 2 wave), this week continues the same two-track story: a clearer production baseline for tooling and a Preview 2 feature that is easy to validate in apps.',
    1774256400, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2026-03-23', 'TechHub',
    'TechHub', '5a8e1eaf15f2733507c22e8967a3516d', ',GitHub Copilot,Copilot Coding Agent,VS Code Copilot Agents,MCP,Azure DevOps,GitHub Actions,GitHub Enterprise Server,Azure AI Foundry,Agent Runtime,OpenTelemetry,Secret Scanning,GitHub Advanced Security,Microsoft Fabric,OneLake,AKS,AI,Azure,.NET,DevOps,ML,Security,Roundups,',
    false, false, true,
    false, false,
    false, false, 4, NULL
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2026-03-16
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', 'Copilot Becomes More Agent-Driven, While Guardrails and Ops Tools Catch Up',
    'This week’s .NET updates split into new features to try and updates to apply. .NET 11 Preview 2 added runtime, observability, and web/data updates, while .NET 10/9/8 servicing focused on secure, stable builds plus an out-of-band macOS debugger hotfix for VS Code users.

<!--excerpt_end-->

## .NET

### .NET 11 Preview 2: runtime, web, and data stack updates

.NET 11 Preview 2 includes updates that show up in applications and pipelines. Runtime work continues on async improvements (V2) plus JIT/VM changes like cached interface dispatch for interface-heavy hot paths. Libraries also get targeted performance improvements (for example, Matrix4x4.GetDeterminant is ~15% faster) and functional additions like generic GetTypeInfo support in System.Text.Json for source-gen and type-metadata scenarios.

SDK changes reduce day-to-day friction: smaller SDK installers on Linux/macOS (useful for development and smaller CI images), improved analyzers, and more warnings/targets to surface issues earlier. Language changes are light for C# and Visual Basic, while F# adds developer-focused updates like overload resolution caching, new preprocessor directives, and new collection functions.

ASP.NET Core and Blazor leaned into built-in platform features. Native OpenTelemetry tracing in ASP.NET Core reduces custom tracing setup, OpenAPI 3.2.0 support updates API description workflows, and there’s a new .NET Web Worker template. Blazor adds TempData for state that survives redirects/navigation. EF Core adds server-side translation for LINQ MaxBy/MinBy plus SQL Server features like DiskANN vector indexes and VECTOR_SEARCH(), along with full-text catalogs/indexes and JSON_CONTAINS().

UI and deployment edges also moved forward. .NET MAUI includes Map control and TypedBinding performance work, immutability annotations for Color/Font, and API consistency (notably VisualStateManager), while Windows Forms/WPF get reliability updates. Container teams also get smaller images (SDK images up to ~17% smaller), which improves pulls and CI throughput. Preview 2 is available via the .NET 11 Preview 2 SDK, Visual Studio 2026 Insiders on Windows, or VS Code with C# Dev Kit for early validation and tracking release notes.

- [.NET 11 Preview 2: New Features and Improvements Across Runtime, SDK, and Libraries](https://devblogs.microsoft.com/dotnet/dotnet-11-preview-2/)

### Built-in Zstandard compression in .NET 11 and ASP.NET Core

.NET 11 adds built-in Zstandard (zstd) support. System.IO.Compression will include zstd alongside gzip/Deflate and Brotli, which removes the need for third-party wrappers when you want zstd’s compression profile and gives framework components a standard implementation to use.

ASP.NET Core also gains zstd as an out-of-the-box HTTP response compression encoding, so teams can evaluate it like gzip or Brotli when balancing payload size vs CPU. The session covers the new APIs for direct use (streams/files) and indirect use (middleware/framework) and includes benchmarks to help decide when zstd fits versus existing algorithms.

- [ASP.NET Community Standup: Zstandard Compression Comes to .NET 11](https://www.youtube.com/watch?v=JDhs-5wVTnw)

### .NET servicing: March security patches and a macOS VS Code debugger hotfix

March 2026 servicing shipped clear “update now” guidance for teams on multiple .NET versions. Patched releases are .NET 10.0.4, .NET 9.0.14, and .NET 8.0.25, with installers/binaries, MCR container images, Linux package instructions, and known-issues pages. Security includes three “.NET Security Feature Bypass” CVEs: CVE-2026-26130 (.NET 10/9/8), CVE-2026-26127 (.NET 10/9), and CVE-2026-26131 (.NET 10 only). The practical guidance is to update SDKs/runtimes on developer machines, build agents, deployed hosts, and base images, then validate against the known-issues lists. The roundup also links servicing-approved-issue queries for ASP.NET Core, EF Core, runtime, WPF, and more so you can find fixes that may affect your applications.

A second servicing item followed: .NET 10.0.5 out-of-band fixed a macOS-only debugger crash regression introduced in .NET 10.0.4 when debugging from VS Code, especially impacting Apple Silicon (ARM64). If affected, install the .NET 10.0.5 SDK, restart VS Code, and confirm with `dotnet --version`. If you are not on macOS or are not seeing the crash, the guidance says staying on 10.0.4 is fine because 10.0.5 targets this issue narrowly.

- [.NET and .NET Framework March 2026 Servicing Updates: Security and Release Roundup](https://devblogs.microsoft.com/dotnet/dotnet-and-dotnet-framework-march-2026-servicing-updates/)
- [.NET 10.0.5 Out-of-Band Release: Debugger Crash Fix for macOS with Visual Studio Code](https://devblogs.microsoft.com/dotnet/dotnet-10-0-5-oob-release-macos-debugger-fix/)

### Other .NET News

NetEscapades.EnumGenerators adjusted its package layout to better match how source generators are consumed. The split adds a metapackage for the default path, a Generators-only package for keeping the generator private (for example, `PrivateAssets="All"`), and a RuntimeDependencies package for option/serialization types that generated code may need. The change addresses missing-type issues when teams exclude runtime assets.

- [Splitting the NetEscapades.EnumGenerators Packages: Architecture and Stable Release Path](https://andrewlock.net/splitting-the-netescapades-enumgenerators-packages-the-road-to-a-stable-release/)

A MonoGame walkthrough provided an end-to-end first project on modern .NET (recommended .NET SDK 10), covering setup, templates, the edit/build/run loop, and 2D basics like sprite rendering and input handling. It also reinforces MonoGame’s cross-platform support on Windows and Linux using Visual Studio or VS Code.

- [Make your First Game with .NET and MonoGame](https://www.youtube.com/watch?v=y631qBfWk_I)',
    'This week’s .NET updates split into new features to try and updates to apply. .NET 11 Preview 2 added runtime, observability, and web/data updates, while .NET 10/9/8 servicing focused on secure, stable builds plus an out-of-band macOS debugger hotfix for VS Code users.',
    1773651600, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2026-03-16', 'TechHub',
    'TechHub', '7d13dd316e35d80bf271bc94f1b4b809', ',GitHub Copilot,Agentic Workflows,VS Code,JetBrains,MCP,OpenTelemetry,GitHub Actions,Azure SRE Agent,Microsoft Agent Framework,Microsoft Foundry,Microsoft Fabric,AKS,.NET 11,CodeQL,Secret Scanning,AI,Azure,.NET,DevOps,ML,Security,Roundups,',
    false, false, true,
    false, false,
    false, false, 4, NULL
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2026-02-23
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2026-02-23', 'roundups', 'Agentic Workflows, AI Integration, and Security-Focused Cloud Developments',
    'Coding updates this week highlight new .NET runtime features, expanded agent workflows in VS Code, and community stories in language development and open source. These changes provide more options for reliable code, stronger automation, and better integration in developer tools.

<!--excerpt_end-->

## Coding

### .NET Runtime and Instrumentation Advances

.NET 11 improves Async/Await performance for high-concurrency applications, reducing overhead and boosting scalability. Developers are given practical advice for using these updates in real-world code. A detailed guide explores System.Diagnostics.Metrics APIs in .NET, comparing standard and observable metric instruments, and suggests when to use push or pull reporting and OpenTelemetry integration.

These instrumentation improvements continue .NET’s movement toward greater observability and diagnostics, following last week’s release highlights.

- [Async Await Just Got A Massive Improvement in .NET](/dotnet/videos/async-await-just-got-a-massive-improvement-in-net)
- [Understanding Observable and Standard Instruments with System.Diagnostics.Metrics APIs](https://andrewlock.net/creating-standard-and-observable-instruments/)

### Visual Studio Code Workflow and Browser Integration

VS Code’s integrated browser brings live preview, real-time debugging, Chrome DevTools, and AI chat features directly into the editor. This update lowers the need for context switching and allows for smarter agent extension development. New browser workflows and agent-management features drive a tighter feedback loop and provide more productive coding experiences.

Live coding challenges experiment with agent workflows and share actionable discoveries for continuous improvement.

- [The Browser in Your Editor: Integrated Web Preview in VS Code](/dotnet/videos/the-browser-in-your-editor-integrated-web-preview-in-vs-code)
- [Live Coding Challenge: Exploring Agent Workflows in Visual Studio Code](/dotnet/videos/live-coding-challenge-exploring-agent-workflows-in-visual-studio-code)

### Open Source Ecosystems and Language Histories

A pair of interviews with Anders Hejlsberg discuss why making TypeScript open source drove growth, trust, and quality—along with the impact of migrating to GitHub for increased openness and transparent, sustainable development. These discussions provide perspective on why open-source processes benefit technical communities, as covered in earlier roundups.

- [Why TypeScript Had to Be Open Source](/dotnet/videos/why-typescript-had-to-be-open-source)
- [Why TypeScript Moved to GitHub in 2014](/dotnet/videos/why-typescript-moved-to-github-in-2014)

### Windows MIDI Services: Next-Gen Music Tech on Windows 11

Windows 11 introduces updated MIDI Services, adding support for MIDI 1.0/2.0, high-resolution data, legacy compatibility, and multi-client MIDI ports. New features include metadata editing, loopback endpoints, communication between apps, better timing, updated drivers, and an open SDK. Future plans include USB audio class, BLE MIDI, and expanded routing features.

- [Windows 11 Introduces Advanced MIDI 2.0 Support for Musicians and Developers](https://blogs.windows.com/windowsexperience/2026/02/17/making-music-with-midi-just-got-a-real-boost-in-windows-11/)

### PowerShell, OpenSSH, and DSC: Long-Term Roadmap

For 2026, the PowerShell, OpenSSH, and DSC roadmap details improvements to PowerShell 7.7—covering script path flexibility, Bash-style aliases, AI-powered scripting assistance, predictive IntelliSense in PSReadLine, and module gallery migration. OpenSSH gains better authentication methods and DSC v3.2 includes Linux/Python adapters. Regular updates and open collaboration continue for module development and automation on both Windows and Linux.

AI-driven scripting connects to wider agent-based process advancements in the industry.

- [PowerShell, OpenSSH, and DSC: Planned Team Investments for 2026](https://devblogs.microsoft.com/powershell/powershell-openssh-and-dsc-team-investments-for-2026/)

### Inside Model Context Protocol: Workflow and Open Source Journey

An interview with David Soria Parra explores the evolution of Model Context Protocol, its challenges, the journey to Linux Foundation, the Python/Azure tech stack, the "Skills" approach, and best practices for open-source collaboration and protocol leadership.

The discussion sheds light on the journey from internal protocol to open source, reflecting the process lessons from earlier MCP updates.

- [Inside MCP: Origin, Workflow, and Future with David Soria Parra](/dotnet/videos/inside-mcp-origin-workflow-and-future-with-david-soria-parra)',
    'Coding updates this week highlight new .NET runtime features, expanded agent workflows in VS Code, and community stories in language development and open source. These changes provide more options for reliable code, stronger automation, and better integration in developer tools.',
    1771837200, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2026-02-23', 'TechHub',
    'TechHub', '3485856dc2f93f236ce0b0107df7fdcb', ',AI,AI Agents,AI Models,Azure,CI/CD,Cloud Reliability,Data Engineering,DevOps,Enterprise Migration,GitHub Copilot,Kubernetes,Microsoft Foundry,ML,Open Source,Roundups,Security,VS Code,Workflow Automation,.NET,',
    false, false, true,
    false, false,
    false, false, 4, NULL
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2026-02-16
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2026-02-16', 'roundups', 'New Developments in Agent Workflows, Unified AI Tools, and Secure Automation',
    'Programming languages and tools see several updates this week, with new features in .NET and TypeScript, and productivity improvements for VS Code.

<!--excerpt_end-->

## Coding

### .NET Ecosystem Updates

.NET 11 Preview 1 brings a range of updates: Zstandard compression, BFloat16 data type, better ZipArchiveEntry operations, new hard-link and crypto APIs, and collection improvements. Runtime features include async main, WebAssembly CoreCLR, interpreter/JIT updates, and more hardware support.

SDK tools see improved device selection (`dotnet run`), easier test syntax, better watch/hot reload, static analyzers, and enhanced MSBuild logging. The languages get new collections (C#), parallel F# builds, and speed increases. ASP.NET Core/Blazor gets a new UI, SignalR, and better cert management. .NET MAUI now uses CoreCLR for Android. Entity Framework Core is updated for complex JSON column types. Both VS 2026 and VS Code C# Dev Kit support these changes.

February’s .NET and .NET Framework servicing update targets supported versions and addresses CVE-2026-21218. Guidance is provided for installation, patch verification, and changelog review. These continue last week’s trend toward platform modernization, with regular previews and focus on compatibility.

- [.NET 11 Preview 1: New Features and Improvements Across the Ecosystem](https://devblogs.microsoft.com/dotnet/dotnet-11-preview-1/)
- [.NET and .NET Framework February 2026 Servicing Releases Update](https://devblogs.microsoft.com/dotnet/dotnet-and-dotnet-framework-february-2026-servicing-updates/)

### TypeScript 6.0 Beta and Language Modernization

TypeScript 6.0 Beta will be the last version built in JavaScript before 7.0 moves to Go. Updates include changes to context-sensitive function declarations, easier Node.js imports, combined resolution/output settings, and deterministic type ordering via `--stableTypeOrdering`.

Several features are now deprecated: strict mode is default, modules align to current standards, legacy settings are dropped, iterables are always available in DOM libraries, and explicit `types` fields are required. Migration is eased with tools like `ts5to6` and configuration tips. Developers are urged to begin testing their codebases and try out 7.0 Go-native builds for feedback and confidence.

These updates continue last week’s focus on rapid TypeScript iteration, improved compatibility with AI code tools, and preparing developer teams for future platform updates.

- [Announcing TypeScript 6.0 Beta: Key Features, Deprecations, and Migration Guide](https://devblogs.microsoft.com/typescript/announcing-typescript-6-0-beta/)

### Visual Studio Code Enhancements: AI Agents and Productivity Tools

VS Code 1.109 adds usability and workflow improvements. You can now "Ask Questions" in the editor, use agent skills for code automation, and run subagents in parallel for advanced tasks. Editor changes include double-click selection of brackets and strings, a browser preview, and upgraded MCP cloud app support.

A video demo shows agent steering in VS Code Insiders, letting you queue and control agent tasks directly for repeatable, controlled workflow automation. These additions build on the theme of expanded automation, agent control, and integration for both new and advanced users.

- [VS Code 1.109 Release Highlights: Editor Improvements & New Features](/vs-code-1109-release-highlights-editor-improvements-and-new-features)
- [Let it Cook: Agent Steering & Queueing in VS Code Insiders](/let-it-cook-agent-steering-and-queueing-in-vs-code-insiders)

### Other Coding News

A recent Rx.NET v7 live session covers new asynchronous APIs and event stream features, adoption recommendations, and future plans, all presented by Rx.NET team engineers.

- [On .NET Live: Rx.NET v7 and Futures](/on-net-live-rxnet-v7-and-futures)',
    'Programming languages and tools see several updates this week, with new features in .NET and TypeScript, and productivity improvements for VS Code.',
    1771232400, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2026-02-16', 'TechHub',
    'TechHub', '407cf56a63529a5b65a1e2551cea90b5', ',AI,AI Agents,Azure,Cloud Computing,Copilot Studio,DevOps,GitHub Copilot,Governance,JetBrains,LLM Fine Tuning,Microsoft Fabric,ML,Observability,OpenAI,Roundups,Security,TypeScript,VS,VS Code,.NET,',
    false, false, true,
    false, false,
    false, false, 4, NULL
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2026-02-09
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2026-02-09', 'roundups', 'Agent Workflows, AI Platform Updates, and Security: GitHub Copilot, Azure, and More',
    'Recent coding highlights include updated AI features, platform improvements, and evolving best practices. VS Code and .NET continue to offer better tools for developers, while documentation and community resources ease migration and onboarding.

<!--excerpt_end-->

## Coding

### Visual Studio Code: Workflow, Multi-Agent, Terminal, and Beginner Experience

VS Code is now more agent-focused, continuing last week’s MCP Apps progress with better local, cloud, and background agent handling. The improved Agent Sessions Welcome Page and subagent tasking features help automate and delegate work, while upgraded UIs make agent management easier.

The January 2026 Insiders Update (v1.109) includes notable changes to the terminal (such as kitty protocol support and better input handling), improved formatting, bug fixes, updated command handling, and enhanced API access. Chat features retain context even after archiving, and custom UI support for MCP Apps gets a boost. Official tutorials walk new users through core features—including IntelliSense, issue management, and setup—connecting with the multi-agent and MCP functionality covered last week.

- [Multi-agent Development in VS Code](/multi-agent-development-in-vs-code)
- [Visual Studio Code January 2026 Insiders Update (v1.109): Terminal and Chat Improvements](https://code.visualstudio.com/updates/v1_109)
- [Learn Visual Studio Code in 15 Minutes: Official Beginner Tutorial](/learn-visual-studio-code-in-15-minutes-official-beginner-tutorial)
- [Learn Visual Studio Code in 15 Minutes: 2026 Official Beginner Tutorial](/learn-visual-studio-code-in-15-minutes-2026-official-beginner-tutorial)

### .NET Platform: Community-Driven Libraries, Roadmaps, and Deployment Changes

The .NET Data Community Standup introduces Microsoft.Extensions.DataIngestion for parsing documents and handling structured and vector workflows, supporting AI/ML cases as seen in recent community projects. Included demos and architecture advice shape next steps.

Blazor Community Standup walks through the .NET 11 ASP.NET Core and Blazor roadmap, continuing last week’s modernization efforts. .NET Framework 3.5 now requires a separate installer in new Windows releases (as of Insider Build 27965), with full support ending in 2029, so teams should start updating. The .NET MAUI Standup discusses hybrid development and ongoing plans, keeping focus on flexible deployment options.

- [.NET Data Community Standup: Introduction to Microsoft.Extensions.DataIngestion](/net-data-community-standup-introduction-to-microsoftextensionsdataingestion)
- [Blazor Community Standup: ASP.NET Core & Blazor Roadmap for .NET 11](/blazor-community-standup-aspnet-core-and-blazor-roadmap-for-net-11)
- [.NET Framework 3.5 Now Requires Standalone Deployment on New Windows Versions](https://devblogs.microsoft.com/dotnet/dotnet-framework-3-5-moves-to-standalone-deployment-in-new-versions-of-windows/)
- [.NET MAUI Community Standup: Live from MAUI Day London](/net-maui-community-standup-live-from-maui-day-london)

### Visual Studio and Editor Extensibility

Visual Studio 2026 supports background loading for MEF-based productivity extensions, shortening startup times and improving reliability. The new Microsoft.VisualStudio.SDK.Analyzers package (v17.7.98) helps detect and correct threading issues. Enable the related feature flag and follow new guides and code reviews for easier async use with extensibility code.

- [Performance Improvements for MEF-Based Visual Studio 2026 Extensions](https://devblogs.microsoft.com/visualstudio/performance-improvements-to-mef-based-editor-productivity-extensions/)

### Software Development Trends and Open Source Tooling

GitHub’s Octoverse 2025 shows TypeScript now leads Python and JavaScript in overall usage—driven partly by its better support for AI code suggestions. Project maintainers should add typing details for safer AI-assisted workflows. Python remains dominant for AI, but more projects are updating packaging, build, typing, release, and container practices to transition from prototyping to production.

Interest in tools like astral-sh/uv highlights the community’s desire for fast, repeatable deployments. Strong documentation and onboarding (e.g., VS Code and First Contributions) support sustainable open source growth and easier onboarding for new contributors.

- [What the Fastest-Growing Tools Reveal About Modern Software Development](https://github.blog/news-insights/octoverse/what-the-fastest-growing-tools-reveal-about-how-software-is-being-built/)

### Observability, Diagnostics, and Application Instrumentation in .NET

Andrew Lock reviews source generators for .NET metric instrumentation using Microsoft.Extensions.Telemetry.Abstractions and System.Diagnostics.Metrics. Source generators reduce repetitive code for metrics but sometimes have limits compared to manual instrumentation. The review offers examples to help devs evaluate trade-offs for reliability and monitoring.

- [Evaluating System.Diagnostics.Metrics Source Generators with Microsoft.Extensions.Telemetry.Abstractions](https://andrewlock.net/creating-strongly-typed-metics-with-a-source-generator/)

### Other Coding News

A technical walk-through looks at updating a WPF WebView2 control when data changes, including cache refresh strategies and how to automate reloads or use the DevTools Protocol for consistency.

- [Reliably Refreshing the WebView2 Control in WPF Applications](https://weblog.west-wind.com/posts/2026/Feb/04/Reliably-Refreshing-the-WebView2-Control)',
    'Recent coding highlights include updated AI features, platform improvements, and evolving best practices. VS Code and .NET continue to offer better tools for developers, while documentation and community resources ease migration and onboarding.',
    1770627600, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2026-02-09', 'TechHub',
    'TechHub', '1db5a032a9524d442f7f1f59826d64a9', ',.NET,AI,AI Orchestration,Azure,Azure Infrastructure,CI/CD,Cloud Native Development,Copilot SDK,Data Engineering,DevOps,GitHub Copilot,Kubernetes,Microsoft Fabric,ML,Model Management,Multi Agent Systems,Roundups,Security,VS Code,',
    false, false, true,
    false, false,
    false, false, 4, NULL
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2026-02-02
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2026-02-02', 'roundups', 'GitHub Copilot, AI Accelerators, and Secure Cloud: The Latest in Platform and Developer Tools',
    'Recent updates in coding focus on expanded language features, developer tools, and practical advice for .NET, C#, Visual Studio, and programming strategies. The emphasis is on maintainability, performance, and immediate modernization tips.

<!--excerpt_end-->

## Coding

### .NET and C# Development: Practical Instrumentation, Performance, and Language Features

Andrew Lock’s guide to System.Diagnostics.Metrics APIs teaches metrics, counters, and monitoring integration for ASP.NET Core (using dotnet-counters, Datadog, and OpenTelemetry). Steve Gordon’s tutorial connects benchmarking tools and practical performance gains, showing how to find memory allocation bottlenecks and differences in reporting tools. Nick Chapsas'' "100 Essential Tips" round up key best practices for C# devs and introduce Collection Expression Arguments for C# 15/.NET 11, bringing easier parameter handling. He also explores nuanced Boolean behavior. Sentry logging integration now improves observability and connects errors, traces, and logs in .NET and MAUI apps.

- [Creating and Consuming Metrics with System.Diagnostics.Metrics APIs in .NET](https://andrewlock.net/creating-and-consuming-metrics-with-system-diagnostics-metrics-apis/)
- [Application Performance Optimisation in .NET: Practical Techniques and Tools](https://www.stevejgordon.co.uk/talk-application-performance-optimisation-in-practice-60-mins)
- [Solving .NET Memory Allocation Discrepancies: The Case of the Missing 18 Bytes](https://www.stevejgordon.co.uk/the-grand-mystery-of-the-missing-18-bytes)
- [100 Essential Tips for Writing Better C# Code](/coding/videos/100-essential-tips-for-writing-better-c-code)
- [Introducing Collection Expression Arguments in C# 15 and .NET 11](/coding/videos/introducing-collection-expression-arguments-in-c-15-and-net-11)
- [The Boolean Trick No C# Developer Knows About](/coding/videos/the-boolean-trick-no-c-developer-knows-about)
- [Integrating Sentry Logging with .NET Applications](https://dotnetfoundation.org/news-events/detail/sponsor-spotlight-sentryblog1)

### Language Design, Tooling Philosophy, and AI in Programming

Insights from Anders Hejlsberg, the creator of C# and TypeScript, highlight the importance of fast feedback, backward compatibility, and open collaboration for large codebases. Interviews outline the migration of TypeScript to Go and discuss the benefits of Go''s garbage-collected architecture. Hejlsberg also notes AI can boost productivity but does not yet provide the determinism compilers require. TypeScript’s mission of adding opt-in static typing and better tooling to JavaScript is clarified.

- [7 Key Lessons from Anders Hejlsberg: Architect of C# and TypeScript](https://github.blog/developer-skills/programming-languages-and-frameworks/7-learnings-from-anders-hejlsberg-the-architect-behind-c-and-typescript/)
- [TypeScript inventor Anders Hejlsberg: AI’s Role in Language Porting and Development Tools](https://devclass.com/2026/01/28/typescript-inventor-anders-hejlsberg-ai-is-a-big-regurgitator-of-stuff-someone-has-done/)
- [Anders Hejlsberg on the Origins of TypeScript and Fixing JavaScript](/coding/videos/anders-hejlsberg-on-the-origins-of-typescript-and-fixing-javascript)

### WebAssembly and Cross-Platform Coding: .NET, Uno, and Microsoft’s Vision

WebAssembly support is expanding, with improved AOT compilation for .NET 10 and multi-threaded workflows using Uno Platform. New surveys show faster browser loads (Chrome, Firefox, Safari), wider runtime adoption (Node.js, Wasmtime, Deno), and broader language support in Wasm 3.0.

- [WebAssembly Adoption Accelerates with Microsoft .NET 10 and Uno Platform Collaboration](https://devclass.com/2026/01/28/webassembly-gaining-adoption-behind-the-scenes-as-technology-advances/)

### Other Coding News

Visual Studio’s January 2026 update brings editor improvements (scrolling, clipboard, suggestions, Markdown preview), driving faster development. The ''winapp'' CLI preview allows simplified access to modern Windows APIs for non-VS projects, lowering the barrier for Windows API adoption from cross-platform environments.

Microsoft will present at NDC London 2026, covering .NET 10, Copilot coding, and performance, and will offer guidance for effective modernization and productivity.

- [Visual Studio January Update — Enhanced Editor Experience](https://devblogs.microsoft.com/visualstudio/visual-studio-january-update-enhanced-editor-experience/)
- [Microsoft Previews Winapp: Simplifying Access to Modern Windows APIs for Developers](https://devclass.com/2026/01/26/microsoft-previews-command-line-tool-created-because-calling-modern-windows-apis-is-too-difficult/)
- [Join Microsoft at NDC London 2026 – Let’s Build the Future of .NET Together](https://devblogs.microsoft.com/dotnet/join-us-at-ndc-london-2026/)',
    'Recent updates in coding focus on expanded language features, developer tools, and practical advice for .NET, C#, Visual Studio, and programming strategies. The emphasis is on maintainability, performance, and immediate modernization tips.',
    1770022800, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2026-02-02', 'TechHub',
    'TechHub', '66869e02ac3da774e97cf5926cd9f8cf', ',.NET,Agentic Workflows,AI,Azure,CLI Automation,Cloud Computing,Compliance,Data Engineering,DevOps,GitHub Copilot,Kubernetes,MCP,ML,Open Source,Roundups,SDKs,Security,',
    false, false, true,
    false, false,
    false, false, 4, NULL
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2026-01-26
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2026-01-26', 'roundups', 'Advances in AI Tooling, Platform Engineering, and Security Shape This Week’s Highlights',
    'Highlights this week include optimized patterns for .NET and SharePoint, new features for React Native Windows, and practical resources for developer engagement and education.

<!--excerpt_end-->

## Coding

### .NET Performance Optimization and Modern Web Development

This week includes a guide on making `IEnumerable<T>` iteration in .NET allocation-free. Andrew Lock details compiler behavior and testing, explaining how Reflection.Emit/DynamicMethod avoids boxing overhead on older runtimes—useful for SDKs and instrumentation. Resources on modern web teaching (Razor Pages and HTMX) show how to build with less JavaScript, leveraging Razor’s separation of logic for maintainable courses and projects.

- [Making foreach on an IEnumerable Allocation-Free in .NET with Reflection and Dynamic Methods](https://andrewlock.net/making-foreach-on-an-ienumerable-allocation-free-using-reflection-and-dynamic-methods/)
- [Teaching Modern Web Development with .NET, Razor Pages, and HTMX](/coding/videos/teaching-modern-web-development-with-net-razor-pages-and-htmx)

### React Native Windows and Cross-Platform App Development

React Native Windows v0.81 introduces features for desktop debugging and accessibility, including Hermes engine support. Early support for DevTools is coming, helping teams with breakpoints and profiling. Recent architecture work prepares the project for a move to “Fabric.” Discussions remain about whether to prefer React Native or MAUI for .NET-based cross-platform development.

- [Microsoft Updates React Native for Windows: Comparing with MAUI for Cross-Platform Development](https://devclass.com/2026/01/20/microsoft-updates-react-native-for-windows-developers-ask-why-not-use-maui/)

### SharePoint Site Optimization Guides

SharePoint site optimization advice covers scaling with site collections/hubs, filtering lists and libraries, improving metadata, auditing custom code, batching API usage, and regular diagnostics. A checklist provides steps for both urgent and longer-term maintenance across large enterprise sites.

- [Performance Optimization Tips for Large SharePoint Sites](https://dellenny.com/performance-optimization-tips-for-large-sharepoint-sites/)

### Other Coding News

A tutorial shows how to use the "Report Issue" feature in VS Code for feature requests, including tips for improving the chance of getting changes reviewed, referencing feedback cycles as a means to better software.

- [How to Request a VS Code Feature (The RIGHT Way)](/coding/videos/how-to-request-a-vs-code-feature-the-right-way)',
    'Highlights this week include optimized patterns for .NET and SharePoint, new features for React Native Windows, and practical resources for developer engagement and education.',
    1769418000, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2026-01-26', 'TechHub',
    'TechHub', '8ddc916e92a7d97706ce999659873870', ',Agentic SDK,App Modernization,CI/CD,Cloud Engineering,Developer Tools,Kubernetes,Microsoft Fabric,Open Source,Workflow Automation,AI,GitHub Copilot,ML,Azure,DevOps,Security,Roundups,.NET,',
    false, false, true,
    false, false,
    false, false, 4, NULL
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2026-01-19
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2026-01-19', 'roundups', 'AI Developer Tools, Security Updates, and Azure Service Improvements This Week',
    'This week’s section features tutorials and maintenance releases supporting cross-platform development and reliable deployments. The articles focus on Android widget development with .NET MAUI and the latest service updates for .NET, helping developers extend, stabilize, and maintain their applications.

<!--excerpt_end-->

## Coding

### Advanced Android Widget Development with .NET MAUI

You can now create native Android widgets in .NET MAUI using C# (AppWidgetProvider), by following a clear step-by-step guide for project setup, structuring code, and using XML layouts.

The resource outlines how to pass data between your main app and widgets (with Preferences and SharedPreferences), trigger widget actions (like counter buttons) using PendingIntent, and manage configuration steps and Android Context for optimized resource use. Options for automatic or custom widget refreshes—like updatePeriodMillis, AlarmManager, and WorkManager—are explained, with example code for connecting Android platform features to your MAUI app.

This topic extends last week’s coverage of Avalonia UI on Linux for MAUI, as more developers adapt MAUI for broader and more unified use across platforms.

- [How to Build Android Widgets with .NET MAUI](https://devblogs.microsoft.com/dotnet/how-to-build-android-widgets-with-dotnet-maui/)

### .NET Servicing Releases for January 2026

January’s .NET servicing releases are out, delivering non-security patches for .NET 10.0 (10.0.2), 9.0 (9.0.12), and 8.0 (8.0.23), covering the runtime, ASP.NET Core, SDKs, WPF, WinForms, and EF Core. The official changelogs and issue lists are linked for details.

Updates are available as Windows and Linux installers, plus refreshed container images, making it easier to keep environments up to date across hosting scenarios. No changes were released for traditional .NET Framework. Teams should review the changes and update as needed for stability.

These releases connect to last week’s content on the .NET roadmap and developer community status.

- [.NET and .NET Framework January 2026 Servicing Releases Updates](https://devblogs.microsoft.com/dotnet/dotnet-and-dotnet-framework-january-2026-servicing-updates/)',
    'This week’s section features tutorials and maintenance releases supporting cross-platform development and reliable deployments. The articles focus on Android widget development with .NET MAUI and the latest service updates for .NET, helping developers extend, stabilize, and maintain their applications.',
    1768813200, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2026-01-19', 'TechHub',
    'TechHub', 'b05c0ac663af31b5f945155eb5dbb044', ',.NET,Agentic AI,Anthropic,Cloud Infrastructure,Containerization,Enterprise,Identity And Access Management,Microsoft Azure,OpenAI,VS Code,AI,GitHub Copilot,ML,Azure,DevOps,Security,Roundups,',
    false, false, true,
    false, false,
    false, false, 4, NULL
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2026-01-12
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'Agentic AI, GitHub Copilot Updates, Azure Platform Features, and Improving Secure Cloud Workflows',
    'This week’s coding news highlights .NET and C# feature releases, advances in modern UI development, cross-platform tooling, and the evolving use of AI-assisted programming languages.

<!--excerpt_end-->

## Coding

### .NET Ecosystem: New Features, Platforms, and Roadmaps

The .NET ecosystem receives new updates on platform support and development features. MongoDB EF Core adds Queryable Encryption and Vector Search to create secure, privacy-friendly apps with advanced semantic search options.

.NET Community Standups discuss roadmap progress. Blazor Standup previews features coming in .NET 11, with an open invitation to shape web UI planning and migration.

Cross-platform development gains more focus. Avalonia Standup demonstrates Avalonia UI as a Linux backend for .NET MAUI, answering requests for broader deployment options.

Distributed app development is featured in Orleans sessions, diving into scalable architecture patterns for maintainable cloud-native applications.

- [Secure and Intelligent: Queryable Encryption and Vector Search in MongoDB EF Core Provider](https://devblogs.microsoft.com/dotnet/mongodb-efcore-provider-queryable-encryption-vector-search/)
- [Blazor Community Standup: Planning the Future of Blazor in .NET 11](/coding/videos/blazor-community-standup-planning-the-future-of-blazor-in-net-11)
- [.NET MAUI on Linux with Avalonia: Community Standup Recap](/coding/videos/net-maui-on-linux-with-avalonia-community-standup-recap)
- [Orleans Deep Dive: Routing, Placement & Balancing](/coding/videos/orleans-deep-dive-routing-placement-and-balancing)
- [ASP.NET Community Standup: What''s Next for Orleans?](/coding/videos/aspnet-community-standup-whats-next-for-orleans)

### UI Development Tools and Open Source Progress

Microsoft open-sources XAML Studio, inviting contributions to fill gaps in Visual Studio designer functionality highlighted during recent developer discussions. The .NET Foundation release focuses on live, visual design for WinUI and MAUI.

Community conversations continue on sustaining frameworks like WinForms and WPF and the need for unified designer experiences. The project activates open innovation within the .NET UI ecosystem.

- [Microsoft Open Sources XAML Studio and Highlights Visual Studio Designer Challenges](https://devclass.com/2026/01/07/microsoft-open-sources-xaml-studio-amid-developer-discontent-with-visual-studio-designers/)

### Language Trends and AI-Assisted Coding

Analysis of programming language usage reveals Copilot’s effect on adoption of typed languages to reduce AI code issues. This backs up previous trends showing TypeScript’s rise and a movement toward stronger type systems for reliable developer contracts.

Tips on exploring gradually typed languages and testing Copilot CLI match ongoing efforts to improve maintainability as AI tools become integral to coding practices.

- [Why AI Is Pushing Developers Toward Typed Languages](https://github.blog/ai-and-ml/llms/why-ai-is-pushing-developers-toward-typed-languages/)

### Other Coding News

VS Live! 2026 preview features accessible learning in .NET, Visual Studio, and AI development. Labs demonstrate Copilot debugging and modern DevOps, delivering practical community training resources.

- [Immersive Developer Learning with Visual Studio, .NET, Azure, and GitHub Copilot: VS Live! 2026 Preview](https://devblogs.microsoft.com/visualstudio/vs-live-2026-immersive-learning-for-vs2026/)',
    'This week’s coding news highlights .NET and C# feature releases, advances in modern UI development, cross-platform tooling, and the evolving use of AI-assisted programming languages.',
    1768208400, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2026-01-12', 'TechHub',
    'TechHub', '2b76c21bdac229b718aa77fc80133012', ',.NET,Agentic AI,AI Driven Automation,C#,Cloud Cost Management,Cloud Security,High Performance Computing,IDE Integration,Microsoft Fabric,Multi Agent Orchestration,Open Source,Semantic Search,AI,GitHub Copilot,ML,Azure,DevOps,Security,Roundups,',
    false, false, true,
    false, false,
    false, false, 4, NULL
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2026-01-05
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'Agentic AI, Context Engineering, and Secure Automation Power Tech Progress',
    'This Coding section features updates for .NET, highlighting advances in .NET 10, opportunities for skill-building, and new resources in AI integration and developer education.

<!--excerpt_end-->

## Coding

### .NET 10 Technical Developments and AI Integration

The latest roundup on the official .NET blog recaps the .NET 10 LTS release and key additions to the runtime, SDK, and C#. Recent features include faster garbage collection, new benchmarking tools, and improved solution management with updated SLNX and CLI capabilities. .NET 10 also supports AI agent development with the Microsoft Agent Framework, upgrades to the MCP server, and instant access to AI via NuGet packages, helping teams use generative AI across both existing and new solutions.

This post continues threads from the past week’s MCP coverage and agent automation, now available through official templates and NuGet for easier use. The Agent Framework and server reduce barriers for customizing enterprise AI agents. Copilot integration, now through enhanced LTS toolchains, connects .NET’s automation approach with that of Visual Studio Code. Articles compare Copilot’s new Ask and Agent Modes, outlining how they fit into .NET development.

Additionally, .NET Aspire 9.3 brings more support for cloud architectures and improved onboarding. The post links to deep dives, community highlights from .NET Conf 2025, security news, and official lifecycle policies, helping developers stay current with migrations and updates.

- [Top .NET Blog Posts of 2025: .NET 10, AI Integrations, Performance, and Tooling](https://devblogs.microsoft.com/dotnet/top-dotnet-blogs-posts-of-2025/)

### .NET Developer Education and Video Resources

Jon Galloway’s annual roundup curates the year’s top videos and live streams for .NET developers. The featured content spans demos of .NET 10 and Visual Studio 2026, Clean Architecture, performance tuning, modular monolith patterns, advanced C# 14 topics, and Blazor with AI integration (including Python and MCP demos). Tutorials cover a range of workflows, including Hands-on with Blazor AI templates, and track .NET’s continued intersections with modern AI.

This complements ongoing themes of open source development, skill advancement, and innovation through community events. Demo sessions, podcasts, and experimental MCP showcases follow up on discussions from the latest podcasts and technical experimentation.

Community Standup events and “Deep .NET” livestreams highlight interactive and transparent development processes. Additional topics include deploying AI locally, using Akka.NET for high-throughput data, and optimizing SQL. All content is freely available on YouTube and Microsoft Learn to help developers build current skills and adopt new .NET concepts.

- [Top .NET Videos and Live Streams of 2025: A Year in Review](https://devblogs.microsoft.com/dotnet/top-dotnet-videos-live-streams-of-2025/)',
    'This Coding section features updates for .NET, highlighting advances in .NET 10, opportunities for skill-building, and new resources in AI integration and developer education.',
    1767603600, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2026-01-05', 'TechHub',
    'TechHub', 'f9cb98fe91667af85a2907bbcb8f04d6', ',.NET 10,Account Recovery,Agentic AI,AI Integrations,CI/CD,Cloud Automation,Data Migration,Developer Workflows,Fuzz Testing,MCP,Microsoft Agent Framework,Semantic Modeling,VS Code,AI,GitHub Copilot,ML,Azure,DevOps,Security,Roundups,.NET,',
    false, false, true,
    false, false,
    false, false, 4, NULL
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2025-12-29
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2025-12-29', 'roundups', 'AI Agent Frameworks, Copilot Automation, and Security Improvements: Weekly Tech Highlights',
    'This week’s coding news discusses how to prepare for future trends while streamlining everyday workflows. Developers are encouraged to consider strategies and resources for building sustainable, effective skills and projects leading into 2026.

<!--excerpt_end-->

## Coding

### Building Confidence for 2026: Trends, Skills, and Open Source Practices

Cassidy Williams’ curation of GitHub Podcast episodes covers approaches to stay up-to-date with future-oriented coding techniques. Topics include technical advancements like Model Context Protocol (MCP)—which supports improved AI tool interoperability and more resilient software models—with practical references like the GitHub MCP server for hands-on experimentation.

These episodes align with ongoing developer roadmap discussions around technologies like .NET 11, emphasizing the benefits of planning and tool selection for future-ready skills. The guidance is practical, connected to recent posts about career development and open source community health.

DIY and open source contributions remain central. The podcast shares examples of how accessible ecosystems support rapid project building. Episodes also highlight best practices for fostering sustainability in open source, referencing lessons from cases like Log4Shell, and encourage contributors to engage through multiple roles, not just code.

Other segments address trends from Octoverse 2025—such as TypeScript’s growth, the adoption of AI-assisted development, and the continued relevance of existing languages. The Home Assistant community project is given as an example of privacy-focused, community-driven software.

Each episode provides actionable advice, from exploring new tech to contributing for long-term community stability—helping developers align skills and projects with emerging trends.

- [5 Podcast Episodes to Help You Build with Confidence in 2026](https://github.blog/open-source/maintainers/5-podcast-episodes-to-help-you-build-with-confidence-in-2026/)

### Developer Workflow Enhancements in Visual Studio Code

Web developers now have access to the Simple Browser in Visual Studio Code, as explained in Justin Chen’s guide. This feature enables the use of browser tabs for local development servers with integrated inspection tools so developers can debug and resolve HTML issues within the editor.

This follows the recent release of VS Code 1.107, which introduced updated features like inline chat and persistent agents for improved task handling. The current focus on web development features means less switching between tools, streamlining work and saving time.

VS Code’s browser supports integration with AI-powered features and code automation to provide responsive suggestions and speed up front-end workflows. Such tools improve productivity, especially during multi-step debugging and streamlined workspace management.

Planned updates will further integrate development and browser features, ensuring consistent improvements for web developers using VS Code.

- [Unlocking the Power of VS Code''s Simple Browser Feature](/ai/videos/unlocking-the-power-of-vs-codes-simple-browser-feature)',
    'This week’s coding news discusses how to prepare for future trends while streamlining everyday workflows. Developers are encouraged to consider strategies and resources for building sustainable, effective skills and projects leading into 2026.',
    1766998800, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2025-12-29', 'TechHub',
    'TechHub', 'ff9ff704389105a87c56b93cd8a616d0', ',Agent Framework,AI Agents,Automation,Azure Databricks,Azure SRE,CI/CD,Load Testing,MCP,Open Source,Supply Chain Security,VS Code,AI,GitHub Copilot,ML,Azure,DevOps,Security,Roundups,.NET,',
    false, false, true,
    false, false,
    false, false, 4, NULL
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2025-12-22
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2025-12-22', 'roundups', 'AI Agents, DevOps Changes, and Platform Security: Weekly Technology Update',
    'VS Code 1.107 launches inline chat editing and persistent local agents. ASP.NET Core kicks off .NET 11 planning with community input opportunities.

<!--excerpt_end-->

## Coding

### .NET Development: Cross-Platform Widgets and ASP.NET Core Roadmaps

A .NET MAUI walkthrough demonstrates how to build interactive iOS widgets leveraging shared .NET logic and integrating Swift for platform-specific needs. ASP.NET Core’s .NET 11 planning is underway, with opportunities for community input and transparent discussions—helping teams prepare for migration and long-term design decisions.

- [Building iOS Widgets with .NET MAUI: From Setup to Interactive Features](https://devblogs.microsoft.com/dotnet/how-to-build-ios-widgets-with-dotnet-maui/)
- [ASP.NET Core Planning Kickoff for .NET 11](/coding/videos/aspnet-core-planning-kickoff-for-net-11)
- [ASP.NET Core Server & APIs Roadmap Discussion for .NET 11](/coding/videos/aspnet-core-server-and-apis-roadmap-discussion-for-net-11)

### Editor Experiences: VS Code and Cursor AI Updates

VS Code 1.107 launches inline chat editing, advanced renaming, and persistent local agents for background tasks. Cursor AI Editor 2.2 introduces a visual workflow designer and quick access to LLM options but continues to draw developer concerns over frequent interface changes and complex pricing. These updates feed into broader conversations about balancing speed, usability, and control.

- [VS Code 1.107 Release Highlights](/coding/videos/vs-code-1107-release-highlights)
- [AI-Driven Cursor Editor Adds Visual Designer Amid Developer Frustrations](https://devclass.com/2025/12/16/cursor-ai-editor-gets-visual-designer-but-bugs-and-ever-changing-ui-irk-developers/)

### Practical Profiling and Feedback Workflows

A new guide details profiling the .NET CLR using C# and Silhouette, removing the dependency on C++ for diagnostics and performance monitoring. Another post explains how Visual Studio’s feedback process uses transparent triage and prioritization to connect developer suggestions directly to product improvements.

- [Creating a .NET CLR profiler with C# NativeAOT and Silhouette](https://andrewlock.net/creating-a-dotnet-profiler-using-csharp-with-silhouette/)
- [How Visual Studio Feedback Improves the Developer Experience](https://devblogs.microsoft.com/visualstudio/behind-the-scenes-of-the-visual-studio-feedback-system/)',
    'VS Code 1.107 launches inline chat editing and persistent local agents. ASP.NET Core kicks off .NET 11 planning with community input opportunities.',
    1766394000, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2025-12-22', 'TechHub',
    'TechHub', 'c6cbf68b3be705abbdd5e17319aa2359', ',Automation,Cloud Computing,Code Review,Data Engineering,Dependabot,Microsoft Fabric,Orchestration,Prompt Engineering,VS Code,AI,GitHub Copilot,ML,Azure,DevOps,Security,Roundups,.NET,',
    false, false, true,
    false, false,
    false, false, 4, NULL
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2025-12-15
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2025-12-15', 'roundups', 'Streamlining Development: Updates in Copilot, AI Platforms, and Secure Automation',
    E'This week''s updates cover .NET networking changes, cross-platform in-app billing, innovations in PC gaming, and new tools in Visual Studio Code for developer error handling and maintainability.

<!--excerpt_end-->

## Coding

### .NET 10 Platform Developments and Servicing Updates

.NET 10 introduced networking features like optional certificate caching and support for more HTTP verbs, continuing last week’s progress in performance and security. TLS 1.3 adoption improves, and .NET 10.0.1 servicing release upholds the recommendation to remain current, with an emphasis on upgrading over older .NET Framework versions.

- [.NET 10 Networking Improvements](https://devblogs.microsoft.com/dotnet/dotnet-10-networking-improvements/)
- [.NET and .NET Framework December 2025 Servicing Updates Recap](https://devblogs.microsoft.com/dotnet/dotnet-and-dotnet-framework-december-2025-servicing-updates/)

### Cross-Platform Client Development with .NET MAUI

A new sample for .NET MAUI demonstrates unified in-app billing across Android, iOS, Mac, and Windows. It provides abstraction layers for different platforms, migration from third-party plugins, and compliance considerations. Code architecture templates and server validation help lay the groundwork for long-term code maintenance.

- [Cross-Platform In-App Billing in .NET MAUI: New Sample Implementation](https://devblogs.microsoft.com/dotnet/cross-platform-billing-dotnet-maui/)

### Windows PC Gaming Development: Tools and Platform Advancements

PC gaming on Windows receives updates that extend support for handheld devices, ARM-based systems, and new rendering technology. Advanced Shader Delivery and Auto Super Resolution bring both improved player experience and performance, in line with wider cross-device support goals.

- [Windows PC Gaming Innovations in 2025: Handheld Devices, Arm Expansion, and DirectX Upgrades](https://blogs.windows.com/windowsexperience/2025/12/09/windows-pc-gaming-in-2025-handheld-innovation-arm-progress-and-directx-advances/)

### Visual Studio Code: Agent HQ, TypeScript 7, and Copilot Changes

Visual Studio Code introduces the Agent HQ feature for managing multiple development agents, answering increased demand for agentic workflows. TypeScript 7 Preview introduces improvements for modern APIs and parallel services. The deprecation of IntelliCode and Copilot changes for free-tier users adjust the available coding assistance tools.

- [VS Code Update Introduces Agent HQ, TypeScript 7 Preview, and Deprecates IntelliCode](https://devclass.com/2025/12/11/vs-code-update-brings-agent-overload-typescript-7-preview-and-the-end-of-intellicode/)

### Other Coding News

Andrew Lock reviews the new Zed editor, offering .NET and Markdown developers alternatives beyond Visual Studio Code. A troubleshooting guide covers proper exception handling strategies for filesystem issues such as the `\\\\.\\\\nul` path, helping developers write more reliable applications.

- [Trying out the Zed editor on Windows for .NET and Markdown](https://andrewlock.net/trying-out-the-zed-editor-on-windows-for-dotnet-and-markdown/)
- [Troubleshooting the `\\\\.\\\\nul` Path Error in Directory Files Lookup](https://weblog.west-wind.com/posts/2025/Dec/08/What-the-heck-is-a-nul-path-and-why-is-it-breaking-my-Directory-Files-Lookup)',
    'This week''s updates cover .NET networking changes, cross-platform in-app billing, innovations in PC gaming, and new tools in Visual Studio Code for developer error handling and maintainability.',
    1765789200, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2025-12-15', 'TechHub',
    'TechHub', '77bcbc596dc8ffac5d06492dc4f2d4a8', ',Agentic AI,AI Agents,Application Modernization,CI/CD,Cloud Automation,Developer Productivity,Microsoft Fabric,OpenAI GPT 5.2,Supply Chain Security,VS Code,AI,GitHub Copilot,ML,Azure,DevOps,Security,Roundups,.NET,',
    false, false, true,
    false, false,
    false, false, 4, NULL
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2025-12-08
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2025-12-08', 'roundups', 'GitHub Copilot Updates, Agent Tools, .NET 10 Release, and Azure Developments',
    'Updates include .NET 10, Visual Studio 2026, cross-platform frameworks, TypeScript compiler improvements, and AI-powered development tools.

<!--excerpt_end-->

## Coding

### .NET 10, Visual Studio 2026, and MAUI Ecosystem Developments

.NET Conf 2025 introduced .NET 10 (with support until 2028), following last week’s modular IDE announcements and tighter Copilot/VS integration. Updates focus on performance, post-quantum cryptography, ASP.NET Core, Blazor, .NET MAUI, and C# 14. Aspire 13 CLI adds support for Python and JavaScript. Copilot and Agent Framework continue to grow alongside agent-based development. Progress in code quality and security guidance supports inclusive and streamlined development, and the MAUI Community Standup explored .NET 10’s impact and upgrade strategy.

- [.NET Conf 2025 Recap: What''s New in .NET 10, Visual Studio 2026, AI, and App Modernization](https://devblogs.microsoft.com/dotnet/dotnet-conf-2025-recap/)
- [.NET MAUI Community Standup - .NET 10 Announcements Roundup](/coding/videos/net-maui-community-standup-net-10-announcements-roundup)

### Practical Guidance and Tooling for .NET Developers

Stephen Toub and Scott Hanselman share recommendations for CancellationToken use in asynchronous .NET code, supporting robust and testable patterns. Age verification for .NET MAUI now aligns with current global regulations, providing platform-specific tips for Android, iOS, and Windows. The latest NetEscapades.EnumGenerators release adds support for [EnumMember] and improved analyzers.

- [Deep Dive into Cancellation Tokens in .NET with Stephen Toub](/coding/videos/deep-dive-into-cancellation-tokens-in-net-with-stephen-toub)
- [Cross-Platform Age Verification in .NET MAUI Applications](https://devblogs.microsoft.com/dotnet/cross-platform-age-verification-dotnet-maui/)
- [Recent updates to NetEscapades.EnumGenerators: [EnumMember] support, analyzers, and bug fixes](https://andrewlock.net/recent-updates-to-netescapaades-enumgenerators/)

### Agentic UI, MCP, and Coding Experience Updates in Microsoft’s Stack

Demonstrations show AG-UI and Blazor enabling interactive web components for agent-based apps. Updated tooling for MCP in VS Code improves developer onboarding, and features like in-depth keyboard shortcut analysis support improved workflow familiarity in Visual Studio 2026.

- [Building Agentic UI with AG-UI and Blazor: ASP.NET Community Standup](/ai/videos/building-agentic-ui-with-ag-ui-and-blazor-aspnet-community-standup)
- [Tooling Support for MCP in Visual Studio Code](/coding/videos/tooling-support-for-mcp-in-visual-studio-code)
- [Making Sense of Keyboard Shortcuts in Visual Studio 2026](https://devblogs.microsoft.com/visualstudio/why-changing-keyboard-shortcuts-in-visual-studio-isnt-as-simple-as-it-seems/)

### TypeScript Compiler and Language Service Modernization

Project Corsa is taking TypeScript''s compiler and language service to native code in version 7, adding multi-threading and more type safety. Preview features are available in VS Code, and migration guidance is provided.

- [Progress on TypeScript 7: Native Compiler and Language Service Updates](https://devblogs.microsoft.com/typescript/progress-on-typescript-7-december-2025/)

### Other Coding News

The VS Code Insiders Podcast is now available, featuring interviews and technical segments about AI, new VS Code features, accessibility, and open-source engagement.

- [Introducing the VS Code Insiders Podcast](https://code.visualstudio.com/blogs/2025/12/03/introducing-vs-code-insiders-podcast)',
    'Updates include .NET 10, Visual Studio 2026, cross-platform frameworks, TypeScript compiler improvements, and AI-powered development tools.',
    1765184400, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2025-12-08', 'TechHub',
    'TechHub', '0b8c9c7156514d156a5bcb6f79d80b38', ',.NET 10,Agent Framework,AI Agents,Automation,Cloud Native,Enterprise AI,Governance,Microsoft Azure,Microsoft Fabric,OpenAI,TypeScript,VS,AI,GitHub Copilot,ML,Azure,DevOps,Security,Roundups,.NET,',
    false, false, true,
    false, false,
    false, false, 4, NULL
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2025-12-01
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'Ignite 2025: Updates in Agentic AI, Cloud, Developer Tools, and Security',
    'This week in Coding covers improved developer tools, new integrations, and best practices for creating sturdy applications. Updates range across IDE enhancements, built-in AI support, and pragmatic architecture advice. With ongoing improvements in Visual Studio 2026 and the Windows 11 developer suite, Microsoft continues to streamline iteration, boost security, and expand AI’s role in everyday coding. In-depth articles on .NET startup routines and sound C# class design reinforce the move toward maintainable, high-quality code in the Microsoft ecosystem.

<!--excerpt_end-->

## Coding

### Modern IDEs and the Windows Developer Experience

Following Visual Studio 2026’s launch last week, more detail is available on how monthly updates let developers manage innovation and stability by choosing between Insiders, Stable, and LTS channels. Major components like .NET and C++ compilers are now modular, supporting independent updates—an advance from prior .NET improvements.

Copilot is integrated further in Visual Studio 2026, including independent Copilot updates for up-to-date AI coding support. This addresses developer needs for fast adaptation to evolving features.

Windows 11’s toolkit also grows: PowerToys and Windows Terminal now add enhanced security and automation, featuring Copilot in Terminal and new command line tools, as recently previewed. The new ''Edit'' terminal utility and MCP Server deployment as AI agent underline Microsoft’s ongoing support for seamless automation and Azure integration.

- [Visual Studio 2026: Modern IDE with Monthly Updates and Flexible Build Tools](https://devblogs.microsoft.com/visualstudio/visual-studio-built-for-the-speed-of-modern-development/)
- [Windows 11 Developer Productivity Tools: WSL, Terminal, PowerToys & Enterprise Security](/coding/videos/windows-11-developer-productivity-tools-wsl-terminal-powertoys-and-enterprise-security)

### .NET Diagnostics and C# Design Discipline

.NET internals and diagnostics remain a focus, with Andrew Lock’s exploration of the .NET boot sequence complementing previous guides on startup and hosting.

Nick Chapsas’ summary on default class sealing practices supports conversations about code maintainability and extensibility, emphasizing the default use of sealed classes and clear extension points—topical as teams seek more robust design approaches.

- [Exploring the .NET Boot Process via Host Tracing](https://andrewlock.net/exploring-the-dotnet-boot-process-via-host-tracing/)
- [Every Class Should Be Sealed in C#](/coding/videos/every-class-should-be-sealed-in-c)',
    'This week in Coding covers improved developer tools, new integrations, and best practices for creating sturdy applications. Updates range across IDE enhancements, built-in AI support, and pragmatic architecture advice. With ongoing improvements in Visual Studio 2026 and the Windows 11 developer suite, Microsoft continues to streamline iteration, boost security, and expand AI’s role in everyday coding. In-depth articles on .NET startup routines and sound C# class design reinforce the move toward maintainable, high-quality code in the Microsoft ecosystem.',
    1764579600, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2025-12-01', 'TechHub',
    'TechHub', '0f9b3a12240c66e6d7e65e4d9455f725', ',Agentic AI,Automation,Cloud Native,Confidential Computing,Data Platform,Developer Productivity,Device Security,Enterprise Migration,Low Code,Microsoft Foundry,Observability,Power Platform,VS,Zero Trust,AI,GitHub Copilot,ML,Azure,DevOps,Security,Roundups,.NET,',
    false, false, true,
    false, false,
    false, false, 4, NULL
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2025-11-24
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2025-11-24', 'roundups', 'Updates in Agent-Based AI, Secure Automation, and Developer Tools Modernization',
    'Coding news this week includes improvements in programming languages, development tools, and platform interoperability. New releases for C#, F#, and .NET focus on modern features and improved expressiveness. Updates in Visual Studio, VS Code, Git, and Windows target code management, collaboration, and administration. Accessibility, accessible design, and educational content continue to help developers at all levels.

<!--excerpt_end-->

## Coding

### Advancements in .NET Languages: C# 14 and F# 10

After last week’s release of .NET 10, C# 14 and F# 10 introduce updated language features. C# 14 adds extension members, a `field` keyword, unbound generics in `nameof`, and more expressive lambda syntax, supporting safer and more consistent code. Migration resources offer help for adapting to these changes.

F# 10 introduces better warning suppression, enhanced property accessor features, and improved computation expressions and scripting performance. These changes support current tooling and offer hints at the direction for .NET 11’s continued compiler improvements.

- [Introducing C# 14: New Language Features and .NET 10 Integration](https://devblogs.microsoft.com/dotnet/introducing-csharp-14/)
- [What''s New in C# 14 and .NET 10](/coding/videos/whats-new-in-c-14-and-net-10)
- [Introducing F# 10: Language Features, Performance, and Tooling in .NET 10](https://devblogs.microsoft.com/dotnet/introducing-fsharp-10/)

### Visual Studio Family: Modernization, Productivity, and Secure Extension Management

Visual Studio 2026 continues its focus on smooth migration, automation, and productivity. Automated dependency checks, project retargeting, and Copilot support streamline the process of updating legacy apps. Stable update, rollback, and repair options support reliability during upgrades.

Visual Studio Code 1.106 debuts the Private Marketplace, giving organizations better control over which extensions are used while reinforcing secure extension management. Accessibility improvements continue to support every developer’s workflow.

- [Effortless Upgrades and Coding Productivity in Visual Studio 2026](https://devblogs.microsoft.com/visualstudio/spend-less-time-upgrading-more-time-coding-in-visual-studio-2026/)
- [Introducing the Visual Studio Code Private Marketplace: Secure Extension Management for Teams](https://code.visualstudio.com/blogs/2025/11/18/PrivateMarketplace)

### Windows Settings and File Explorer: Developer-Centric Enhancements

Windows updates this week address the needs of developers with tools for managing large projects. The Advanced Settings page and long path support resolve issues in handling more complex codebases. Integration with Git directly in File Explorer underlines Windows’ continuing commitment to supporting version control at the OS level.

- [What''s New in Windows Settings for Developers: Advanced Settings, Long Path Support, and Git Integration](/coding/videos/whats-new-in-windows-settings-for-developers-advanced-settings-long-path-support-and-git-integration)
- [What''s New in Windows Settings for Developers: Advanced Settings, Long File Paths, and Git Integration](/coding/videos/whats-new-in-windows-settings-for-developers-advanced-settings-long-file-paths-and-git-integration)

### Git 2.52: Version Control, Performance, and Migration

Git 2.52 brings further improvements for managing large and legacy repositories. Features like ‘git last-modified’ support better traceability, while geometric repacking and updated tools for large codebases fulfill needs highlighted in recent coverage. Plans to move to SHA-256 and more Rust components demonstrate a continued commitment to security and maintainable workflows.

- [What''s New in Git 2.52: Features and Performance Enhancements](https://github.blog/open-source/git/highlights-from-git-2-52/)

### AI-Enhanced, Cross-Platform Development with Uno Platform

Uno Platform continues the trend of AI-driven cross-platform development. Hot Design and Hot Reload for Studio, support for .NET 10, and Figma integration make it easier for designers and developers to work together and move from design to code more efficiently.

- [Building Cross-Platform .NET Apps with Uno Platform and Contextual AI](/ai/videos/building-cross-platform-net-apps-with-uno-platform-and-contextual-ai)

### Other Coding News

VS Code’s accessibility improvements build on earlier work, helping developers with different needs be more productive. GitHub’s open-source Annotation Toolkit for Figma enables better communication in design-to-code workflows, reinforcing shared standards and compliance.

The .NET Conf Student Zone 2025 showcases the ongoing commitment to practical education, supporting upskilling with hands-on content.

- [Accessibility in Visual Studio Code: Insights from Megan Rogge](/coding/videos/accessibility-in-visual-studio-code-insights-from-megan-rogge)
- [Enhance Design-to-Code Collaboration with GitHub''s Annotation Toolkit](https://github.blog/enterprise-software/collaboration/level-up-design-to-code-collaboration-with-githubs-open-source-annotation-toolkit/)
- [.NET Conf Student Zone 2025](/ai/videos/net-conf-student-zone-2025)',
    'Coding news this week includes improvements in programming languages, development tools, and platform interoperability. New releases for C#, F#, and .NET focus on modern features and improved expressiveness. Updates in Visual Studio, VS Code, Git, and Windows target code management, collaboration, and administration. Accessibility, accessible design, and educational content continue to help developers at all levels.',
    1763974800, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2025-11-24', 'TechHub',
    'TechHub', '82809ed89f053d40f9bc9820b14bb922', ',.NET,AI Agents,Cloud Infrastructure,Data Governance,IDE Integration,Model Deployment,Post Quantum Cryptography,VS,VS Code,Workflow Automation,AI,GitHub Copilot,ML,Azure,DevOps,Security,Roundups,',
    false, false, true,
    false, false,
    false, false, 4, NULL
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2025-11-17
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2025-11-17', 'roundups', 'AI-Integrated Developer Tools, .NET 10 Release, and Cloud Updates Enhance Modern Workflows',
    'This week’s .NET announcements spotlight the general availability of .NET 10, progress in the ecosystem, improved developer tools, advanced security measures, container support, and better workflow orchestration. Community efforts continue to highlight practical migration paths, inclusive development resources, and hands-on learning for modern .NET projects.

<!--excerpt_end-->

## Coding

### .NET 10 Platform Release and Ecosystem Advances

The .NET 10 release marks the new long-term support baseline through 2028, unifying ASP.NET Core, Blazor, MAUI, EF Core 10, C# 14, and F# 10 under one production-ready umbrella. Updates like a new JIT compiler, enhanced NativeAOT, quantum-resistant crypto features, and improved NuGet package security expand on performance and reliability themes. Model Context Protocol (MCP) support and agent frameworks are now central parts of the platform.

Blazor and ASP.NET Core updates—plus OpenAPI 3.1 and improved authentication—continue the push toward modern web capabilities. .NET Aspire strengthens orchestration and Azure deployment options, while Visual Studio 2026 and the enhanced CLI increase productivity and workflow efficiency.

- [Announcing .NET 10: A Major Release for Modern, Productive, and AI-Powered Development](https://devblogs.microsoft.com/dotnet/announcing-dotnet-10/)
- [.NET 10 Migration and New Features for Enterprise Blazor Applications](/coding/videos/net-10-migration-and-new-features-for-enterprise-blazor-applications)
- [Generating Full-Stack .NET Apps with Uno Platform and AI](/ai/videos/generating-full-stack-net-apps-with-uno-platform-and-ai)
- [.NET Framework 4.8 to .NET 9 Step by Step Migration Guide](/coding/videos/net-framework-48-to-net-9-step-by-step-migration-guide)
- [What''s New in NuGet: Security, AI, and Modern Package Management for .NET](/ai/videos/whats-new-in-nuget-security-ai-and-modern-package-management-for-net)
- [Welcome to .NET 10 & Visual Studio 2026!](/coding/videos/welcome-to-net-10-and-visual-studio-2026)
- [Performance Improvements in .NET 10](/coding/videos/performance-improvements-in-net-10)
- [Build Better Web Apps with Blazor in .NET 10](/coding/videos/build-better-web-apps-with-blazor-in-net-10)
- [Modern C# Features to Enhance Your Coding Habits](/coding/videos/modern-c-features-to-enhance-your-coding-habits)
- [What''s New in C# 14](/coding/videos/whats-new-in-c-14)
- [What''s New in ASP.NET Core for .NET 10](/coding/videos/whats-new-in-aspnet-core-for-net-10)
- [What''s New in Containers for .NET 10](/azure/videos/whats-new-in-containers-for-net-10)
- [What''s New in .NET MAUI](/coding/videos/whats-new-in-net-maui)
- [Clean Architecture with ASP.NET Core 10](/coding/videos/clean-architecture-with-aspnet-core-10)
- [Ship Faster with .NET MAUI: Real-World Pitfalls and Solutions](/coding/videos/ship-faster-with-net-maui-real-world-pitfalls-and-solutions)
- [Modern Windows Development with .NET](/coding/videos/modern-windows-development-with-net)
- [New dotnet test Experience with Microsoft.Testing.Platform](/coding/videos/new-dotnet-test-experience-with-microsofttestingplatform)
- [DataMountain: .NET Data Warehousing That Beats C++ Performance](/coding/videos/datamountain-net-data-warehousing-that-beats-c-performance)
- [OpenAPI.NET v2 & v3 Major Release: The Biggest Update Ever](https://devblogs.microsoft.com/openapi/openapi-net-release-announcements/)
- [Simplifying .NET with ''dotnet run file.cs''](/coding/videos/simplifying-net-with-dotnet-run-filecs)
- [.NET and .NET Framework November 2025 Servicing Releases Updates](https://devblogs.microsoft.com/dotnet/dotnet-and-dotnet-framework-november-2025-servicing-updates/)

### .NET Aspire: Cloud-Native Orchestration and Distributed Workflows

Aspire remains central for .NET orchestration, with enhanced documentation and cross-language compatibility in hybrid environments. Updated resources detail configuration for Python, Node.js, and non-.NET applications, keeping modular and multi-team architectures in focus. New resources, dashboards, and onboarding solutions support multi-repo and enterprise scenarios.

- [Simplifying Cloud-Native Development with .NET Aspire](/azure/videos/simplifying-cloud-native-development-with-net-aspire)
- [Taking .NET out of .NET Aspire: Working with Non-.NET Applications](/coding/videos/taking-net-out-of-net-aspire-working-with-non-net-applications)
- [Deep Dive: Extending and Customizing Aspire](/azure/videos/deep-dive-extending-and-customizing-aspire)
- [Aspire Unplugged: David Fowler Discusses .NET Aspire’s Vision and Future](/azure/videos/aspire-unplugged-david-fowler-discusses-net-aspires-vision-and-future)
- [Windows 365 Meets .NET Aspire: Boosting Multi-Repo Microservice Productivity](/coding/videos/windows-365-meets-net-aspire-boosting-multi-repo-microservice-productivity)

### Visual Studio 2026 and Developer Productivity Tools

The Visual Studio 2026 preview introduces a new user experience, accessibility improvements, and expanded customization options, reinforcing the focus on productivity. Integrated AI features and support for .NET 10 enhance debugging and profiling tools for practical application.

- [A First Look at the Refreshed UX in Visual Studio 2026](https://devblogs.microsoft.com/visualstudio/a-first-look-at-the-all%e2%80%91new-ux-in-visual-studio-2026/)
- [Visual Studio Debugger: Advanced Techniques](/ai/videos/visual-studio-debugger-advanced-techniques)
- [Real-World .NET Profiling with Visual Studio](/coding/videos/real-world-net-profiling-with-visual-studio)

### AI, Automation, and API Integration in .NET

AI workflow guidance and MCP/api integration themes continue this week, with practical tutorials for retrofitting APIs, using MCP via NuGet, and enhancing productivity through scripting. These updates expand prototyping and automation efforts highlighted previously.

- [Enhancing Existing .NET REST APIs with Model Creation Protocol (MCP) and AI](/ai/videos/enhancing-existing-net-rest-apis-with-model-creation-protocol-mcp-and-ai)
- [What''s New in NuGet: Security, AI, and Modern Package Management for .NET](/ai/videos/whats-new-in-nuget-security-ai-and-modern-package-management-for-net)
- [Simplifying .NET with ''dotnet run file.cs''](/coding/videos/simplifying-net-with-dotnet-run-filecs)

### Other Coding News

.NET Community Toolkits and the latest MAUI/Windows releases integrate the new .NET launches. Coverage on decision records, Rx.NET status, and performance frameworks (DataMountain, terrain simulation) stays relevant for modular, high-performance engineering. Sustainability and real-world learning feature with topics like MAUI troubleshooting, carbon-aware computing, F# adoption, and IoT solutions with Raspberry Pi. .NET Foundation status and open source initiatives remain visible.

- [Exploring the .NET Community Toolkits: MAUI, Windows, and More](/coding/videos/exploring-the-net-community-toolkits-maui-windows-and-more)
- [Decision Records: Understanding Why Those Decisions Were Made in .NET](/coding/videos/decision-records-understanding-why-those-decisions-were-made-in-net)
- [Rx.NET Status and Roadmap Overview](/coding/videos/rxnet-status-and-roadmap-overview)
- [DataMountain: .NET Data Warehousing That Beats C++ Performance](/coding/videos/datamountain-net-data-warehousing-that-beats-c-performance)
- [High-Performance Terrain Simulations in .NET](/coding/videos/high-performance-terrain-simulations-in-net)
- [Ship Faster with .NET MAUI: Real-World Pitfalls and Solutions](/coding/videos/ship-faster-with-net-maui-real-world-pitfalls-and-solutions)
- [Carbon Aware Computing with .NET Open Source Libraries for Sustainable Applications](/azure/videos/carbon-aware-computing-with-net-open-source-libraries-for-sustainable-applications)
- [Smatterings of F#: Integrating F# in a C#-Focused World](/coding/videos/smatterings-of-f-integrating-f-in-a-c-focused-world)
- [Understanding Nullable Reference Types in .NET](/coding/videos/understanding-nullable-reference-types-in-net)
- [Building Modern CLI Apps in .NET: Libraries, Patterns, and Packaging](/coding/videos/building-modern-cli-apps-in-net-libraries-patterns-and-packaging)
- [Real-World .NET Profiling with Visual Studio](/coding/videos/real-world-net-profiling-with-visual-studio)
- [If .NET Brewed Beer: IoT Brewing Automation with Raspberry Pi](/coding/videos/if-net-brewed-beer-iot-brewing-automation-with-raspberry-pi)
- [The Future of Python and AI with Guido van Rossum](/ai/videos/the-future-of-python-and-ai-with-guido-van-rossum)
- [State of the .NET Foundation and Advances in .NET Open Source](/coding/videos/state-of-the-net-foundation-and-advances-in-net-open-source)',
    'This week’s .NET announcements spotlight the general availability of .NET 10, progress in the ecosystem, improved developer tools, advanced security measures, container support, and better workflow orchestration. Community efforts continue to highlight practical migration paths, inclusive development resources, and hands-on learning for modern .NET projects.',
    1763370000, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2025-11-17', 'TechHub',
    'TechHub', '88230226d54952a72ffe2b2bd7395d17', ',.NET 10,AI Agents,Cloud Native,Code Review,Enterprise Automation,Kubernetes,MCP,Observability,Prompt Engineering,VS,AI,GitHub Copilot,ML,Azure,DevOps,Security,Roundups,.NET,',
    false, false, true,
    false, false,
    false, false, 4, NULL
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2025-11-10
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2025-11-10', 'roundups', 'Updates in AI-Enabled Development, DevOps Automation, and Cloud Security',
    'This section highlights new developments in programming languages and frameworks following the recent .NET Conf 2025 preview. Updates for .NET 10, Visual Studio 2026, and supporting tools reinforce cloud-native, AI-integrated, and modular design approaches.

<!--excerpt_end-->

## Coding

### .NET Ecosystem: From .NET 10 and Visual Studio 2026 to EF 10 and MAUI

.NET Conf 2025, running November 11–13, features tracks on security, NuGet, DevOps, and migrations to expedite adoption of .NET 10 and related AI tools. Demos for Model Context Protocol and Agent Framework support best practices for cloud-native, AI-enabled apps. Entity Framework 10’s release is spotlighted in the .NET Data Community Standup, including upgrade steps and new features. The .NET MAUI Standup marks Grial’s ten-year anniversary, tracing its development from Xamarin to .NET 10 controls.

- [.NET Conf 2025: Launching .NET 10, Visual Studio 2026, and the Future of Cloud-Native and AI Development](https://devblogs.microsoft.com/dotnet/get-ready-for-dotnet-conf-2025/)
- [.NET Data Community Standup: EF 10 Release Celebration](/coding/videos/net-data-community-standup-ef-10-release-celebration)
- [.NET MAUI Community Standup: 10 Years of Grial](/coding/videos/net-maui-community-standup-10-years-of-grial)

### Innovations in TypeScript, Language Trends, and AI Integration

TypeScript remains the leading language on GitHub, a trend confirmed by architect Anders Hejlsberg. Its presence in React, Angular, and SvelteKit continues to grow, while migration to Go improves compiler performance and reliability. AI features increasingly shape language tools and design standards, with Model Context Protocol efforts driving further development of AI-ready programming environments.

- [TypeScript’s Dominance in the AI Era: Insights from Lead Architect Anders Hejlsberg](https://github.blog/developer-skills/programming-languages-and-frameworks/typescripts-rise-in-the-ai-era-insights-from-lead-architect-anders-hejlsberg/)
- [Anders Hejlsberg on Octoverse 2025: TypeScript Evolution, Go, and AI in Development](/ai/videos/anders-hejlsberg-on-octoverse-2025-typescript-evolution-go-and-ai-in-development)

### .NET Features: Reflection Improvements and Troubleshooting Runtime Issues

.NET 10 preview includes [UnsafeAccessorType] for easier reflective programming, enabling more direct access to private and internal members in code libraries and tests. It now supports string-based signatures for streamlined cross-assembly interactions. A troubleshooting guide for .NET 8 covers Windows Service start issues caused by version mismatches, advising configuration adjustments and deployment best practices.

- [Easier Reflection with [UnsafeAccessorType] in .NET 10](https://andrewlock.net/exploring-dotnet-10-preview-features-9-easier-reflection-with-unsafeaccessortype/)
- [.NET 8 Windows Service Fails to Start Due to .NETCore.App Version Mismatch](https://techcommunity.microsoft.com/t5/net-runtime/net-runtime-issues-application-not-starting-up/m-p/4466585#M773)

### Rethinking Software Architecture: Concepts, Synchronizations, and Modular Design

MIT research proposes a software architecture built on ''concepts'' and ''synchronizations'', aiming for applications with encapsulated state and clear behaviors to enhance modularity and maintainability. Interactions are explicit with defined error flows—relevant to scalable, complex systems. Guides for web application development emphasize security, user focus, performance, scalability, and cross-platform design to facilitate well-structured, maintainable codebases.

- [MIT Researchers Propose a New Software Architecture with Concepts and Synchronizations](https://devops.com/mit-researchers-propose-a-new-way-to-build-software-that-actually-makes-sense/)
- [5 Pillars of Successful Web App Development](https://devops.com/5-pillars-of-successful-web-app-development/)',
    'This section highlights new developments in programming languages and frameworks following the recent .NET Conf 2025 preview. Updates for .NET 10, Visual Studio 2026, and supporting tools reinforce cloud-native, AI-integrated, and modular design approaches.',
    1762765200, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2025-11-10', 'TechHub',
    'TechHub', '45a2da6ef3ade705dbf84e65aa481b31', ',.NET,AI Agents,CI/CD,Cloud Native,Containerization,Enterprise AI,Kubernetes,Microsoft Fabric,TypeScript,Workflow Automation,AI,GitHub Copilot,ML,Azure,DevOps,Security,Roundups,',
    false, false, true,
    false, false,
    false, false, 4, NULL
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2025-11-03
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2025-11-03', 'roundups', 'Agents, Automation, and AI: A New Week for GitHub Copilot and Cloud Platforms',
    'This week, developers benefited from future-focused events, hands-on tutorials, and ongoing ecosystem discussions. New resources help developers build their skills, contribute to open-source projects, and explore tooling in both Microsoft and open-source environments. The upcoming .NET Conf introduces new technologies, while the GitHub Game Off 2025 inspires creative game design. Tutorials expand tool customization, and funding analysis continues the dialogue around open-source sustainability.

<!--excerpt_end-->

## Coding

### .NET and Visual Studio: Events and Ecosystem Evolution

In anticipation of .NET Conf 2025, developers look forward to previews of Visual Studio 2026, .NET 10, and broader Copilot support. These sessions highlight easier upgrades and expanded cross-platform, AI-powered coding techniques. The sustainability conversation, guided by Nick Chapsas, addresses open-source library contributions, volunteer burnout, and donor options—following the recent rollout of NuGet.org Sponsorships.

- [Join .NET Conf to Explore Visual Studio 2026 and .NET Innovation](https://devblogs.microsoft.com/visualstudio/join-us-at-net-conf-dive-into-the-future-of-development-with-visual-studio-2026/)
- [.NET Libraries: Monetization Models and Open Source Challenges](/coding/videos/net-libraries-monetization-models-and-open-source-challenges)

### PowerToys and Windows 11: Customization and Productivity Automation

Building on previous extension and toolchain updates, Kayla Cinnamon’s PowerToys Command Palette guide introduces end-to-end development, packaging, and testing in Visual Studio. These tutorials are suitable for both beginner and advanced users looking to boost productivity through custom tooling.

Dellenny’s walkthrough on automating Windows 11 virtual desktops with PowerShell and command-line tools extends earlier workflow enhancements, giving users more control over workspace management.

- [Getting Started with PowerToys Command Palette Extension Development](/coding/videos/getting-started-with-powertoys-command-palette-extension-development)
- [Automating Virtual Desktops in Windows 11 with PowerShell and Command-Line Tools](https://dellenny.com/automating-windows-11-virtual-desktop-management-via-scripting-command-line/)

### GitHub Game Off 2025: Programming Meets Creativity

GitHub Game Off 2025 invites participants to explore the theme “WAVES” using any technology stack. The event continues last week’s onboarding and versioning discussions, encouraging collaborative workflows and creative competition—participants are welcome to use Copilot or other tools during the month-long jam.

- [Announcing the GitHub Game Off 2025: Theme—WAVES](https://github.blog/company/github-game-off-2025-theme-announcement/)',
    'This week, developers benefited from future-focused events, hands-on tutorials, and ongoing ecosystem discussions. New resources help developers build their skills, contribute to open-source projects, and explore tooling in both Microsoft and open-source environments. The upcoming .NET Conf introduces new technologies, while the GitHub Game Off 2025 inspires creative game design. Tutorials expand tool customization, and funding analysis continues the dialogue around open-source sustainability.',
    1762160400, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2025-11-03', 'TechHub',
    'TechHub', '7cd94ddf4b8f9a61c8e2a3e3d7ff2b33', ',.NET,AI Agents,AI Security,App Modernization,Developer Tools,Generative AI,Microsoft Fabric,Open Source,Quantum Computing,Vector Search,VS Code,AI,GitHub Copilot,ML,Azure,DevOps,Security,Roundups,',
    false, false, true,
    false, false,
    false, false, 4, NULL
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2025-10-27
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2025-10-27', 'roundups', 'Agentic AI Advancements, GitHub Copilot Progress, Hybrid DevOps, and Secure Cloud Updates',
    'Coding resources this week provide guides and new tools for smoother workflows, easier migration, and new hands-on experience. There’s a strong sense of continuity in the ecosystem, with changes to extension support, open source funding options, and game development help for C#, Godot, and distributed .NET caching. Tutorials span beginner-friendly introductions to advanced debugging and prototyping, assisting both new and experienced developers.

<!--excerpt_end-->

## Coding

### Godot Engine with .NET and C#

Picking up from last week''s .NET and workflow tutorials, this week the Godot C# Essentials series wraps up with tips for designing responsive UIs using Control Nodes, containers, and signals—useful for features like crafting or inventory screens. Fresh articles explain event-driven code, Area3D collisions, and how to use signals for input, broadening strategies for prototyping.

Additional material on CSG nodes assists with 3D game development. Beginners have access to thorough onboarding guides for installing Godot with .NET, setting up VS Code and key extensions, and adding GitHub version control—creating a smooth path to professional workflows.

Core guides reinforce onboarding, covering player controls, user interfaces, and C# scripting integration. Tutorials on scripting basics—input mapping, movement, lifecycle methods, inspector variables, and debugging in VS Code—reflect last week''s focus on tool improvement and step-by-step learning.

- [Building Responsive UI in Godot C# with Control Nodes](/coding/videos/building-responsive-ui-in-godot-c-with-control-nodes)
- [Interactions in Godot C#: Handling Player Input with Signals and Collision Detection](/coding/videos/interactions-in-godot-c-handling-player-input-with-signals-and-collision-detection)
- [Prototyping Game Environments in Godot C# with CSG Nodes](/coding/videos/prototyping-game-environments-in-godot-c-with-csg-nodes)
- [Scenes and Nodes - The Core Building Blocks in Godot with C# (Part 4 of 9)](/coding/videos/scenes-and-nodes-the-core-building-blocks-in-godot-with-c-part-4-of-9)
- [Installing Godot with .NET and Setting Up VS Code for C# Game Development](/coding/videos/installing-godot-with-net-and-setting-up-vs-code-for-c-game-development)
- [Introduction to Game Development with C# in Godot: Beginner Essentials](/coding/videos/introduction-to-game-development-with-c-in-godot-beginner-essentials)
- [Scripting Basics in Godot: Writing and Attaching C# Scripts (Part 5 of 9)](/coding/videos/scripting-basics-in-godot-writing-and-attaching-c-scripts-part-5-of-9)
- [Engine Overview: Navigating the Godot Editor with C# (Part 3 of 9)](/coding/videos/engine-overview-navigating-the-godot-editor-with-c-part-3-of-9)
- [Debugging Godot C# Games with Visual Studio Code (Godot Series, Part 6)](/coding/videos/debugging-godot-c-games-with-visual-studio-code-godot-series-part-6)

### .NET Ecosystem: Migration, Sponsorship, and Caching

Coming after last week’s .NET 10 RC2 and security updates, Visual Studio 2026 now introduces an improved compatibility model for easier extension migration. New API versioning helps extension developers reduce maintenance, progressing the workflow upgrade story.

NuGet.org launches a Sponsorship feature, allowing open source maintainers to share direct links to funding platforms—continuing discussion from last week about sustainability in the ecosystem.

Distributed caching in .NET gains a new option: Microsoft.Extensions.Caching.Postgres, which performs well and includes features like unlogged tables, supporting scalability and reliability in cloud applications as highlighted in previous updates.

- [Effortless Extension Migration in Visual Studio 2026: Modern Compatibility Model for Developers](https://devblogs.microsoft.com/visualstudio/modernizing-visual-studio-extension-compatibility-effortless-migration-for-extension-developers-and-users/)
- [Announcing Sponsorship on NuGet.org](https://devblogs.microsoft.com/dotnet/announcing-sponsorship-on-nugetdotorg-for-maintainer-appreciation/)
- [Postgres as a Distributed Cache Unlocks Speed and Simplicity for Modern .NET Workloads](https://techcommunity.microsoft.com/t5/microsoft-developer-community/postgres-as-a-distributed-cache-unlocks-speed-and-simplicity-for/ba-p/4462139)

### ASP.NET Core Endpoint Management

Updates in security and routing include new guides on adding metadata to ASP.NET Core endpoints and managing fallback routes. These tutorials deliver practical examples and match ongoing recommendations for robust and secure platform development.

- [Adding Metadata to Fallback Endpoints in ASP.NET Core](https://andrewlock.net/adding-metadata-to-fallback-endpoints-in-aspnetcore/)

### Other Coding News

Developer stories and DIY toolmaking continue from last week, highlighted by a GitHub Podcast discussing motivations for building custom utilities and Copilot’s role in supporting those efforts. The episode extends coverage on workflow personalization, toolchain improvement, and script development.

- [Building Tools and the Future of DIY Development: GitHub Podcast Episode 3](/ai/videos/building-tools-and-the-future-of-diy-development-github-podcast-episode-3)',
    'Coding resources this week provide guides and new tools for smoother workflows, easier migration, and new hands-on experience. There’s a strong sense of continuity in the ecosystem, with changes to extension support, open source funding options, and game development help for C#, Godot, and distributed .NET caching. Tutorials span beginner-friendly introductions to advanced debugging and prototyping, assisting both new and experienced developers.',
    1761555600, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2025-10-27', 'TechHub',
    'TechHub', '66c23857235bdcf5419a81ee64e79387', ',.NET,Agent Framework,AI Agents,Cloud Native,Copilot Studio,Hybrid Cloud,Observability,OpenTelemetry,Retrieval Augmented Generation,Supply Chain Security,VS Code,AI,GitHub Copilot,ML,Azure,DevOps,Security,Roundups,',
    false, false, true,
    false, false,
    false, false, 4, NULL
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2025-10-20
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2025-10-20', 'roundups', 'Updated AI Tools, Open-Source Security, and Azure Integration',
    'The .NET platform saw the arrival of .NET 10 RC2, new security patches, and updated workflows for automating design and console interfaces, laying the groundwork for a modern and secure application environment.

<!--excerpt_end-->

## Coding

### .NET Platform Updates and Security Maintenance

.NET 10 RC2 is ready for use in production, bringing features like upgraded microphone control, SafeAreaEdges in .NET MAUI, XAML source generation, platform compatibility improvements, extended JSON support in EF Core, and enhanced support for .NET MSBuild tasks. Full details are available in the release notes. Developers are encouraged to validate applications and check documentation before the GA release.

Concurrent with other platform and debugging updates, RC2 continues to improve reliability.

Security updates for October 2025 address issues in .NET 8.0, 9.0, and legacy .NET Framework versions, patching vulnerabilities like information disclosure, feature bypass concerns, denial of service, and remote code execution. The fixes extend to runtime, SDK, ASP.NET Core, and container images; teams should update promptly and use provided support materials.

- [Announcing .NET 10 Release Candidate 2](https://devblogs.microsoft.com/dotnet/dotnet-10-rc-2/)
- [.NET and .NET Framework October 2025 Servicing Updates: Security Fixes and Release Details](https://devblogs.microsoft.com/dotnet/dotnet-and-dotnet-framework-october-2025-servicing-updates/)

### Developer Workflow Enhancements: Design, Console UIs, and Tutorials

A live VS Code session demonstrates how to connect Figma MCP Server with Code Connect, making it easier to synchronize design tokens and produce up-to-date code from designs in Figma to Visual Studio Code.

This aligns with recent MCP updates in VS Code, providing more custom and integrated workflows.

A new guide highlights how to use RazorConsole to build interactive, visually rich .NET console apps, showing how to integrate RazorConsole, manage layouts, and create modern UIs. This encourages teams to bring new capabilities to CLI tools.

Building on last week’s discussions on debugging and workflow improvement, these tools enhance .NET development experiences and customizability.

- [VS Code Live: Integrating Figma MCP Server with Code Connect](/coding/videos/vs-code-live-integrating-figma-mcp-server-with-code-connect)
- [Building the Coolest Console Apps in .NET](/coding/videos/building-the-coolest-console-apps-in-net)',
    'The .NET platform saw the arrival of .NET 10 RC2, new security patches, and updated workflows for automating design and console interfaces, laying the groundwork for a modern and secure application environment.',
    1760950800, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2025-10-20', 'TechHub',
    'TechHub', '82ef282d98f5fc5088a95fd6784d1dc2', ',Agentic AI,Automation,Cloud Infrastructure,Developer Workflows,MCP,Open Source,Quantum Resilient Hardware,VS Code,AI,GitHub Copilot,ML,Azure,DevOps,Security,Roundups,.NET,',
    false, false, true,
    false, false,
    false, false, 4, NULL
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2025-10-13
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2025-10-13', 'roundups', 'AI Agents and Automated Workflows Update Developer Tools and Cloud Platforms',
    'Coding news this week centers on practical updates for developer workflows, including platform releases, debugging, open source compliance, AI-supported code assistance, productivity tools, improved .NET memory management, and guidance for licensing in long-term open source projects.

<!--excerpt_end-->

## Coding

### Visual Studio Code: AI, Copilot, and MCP Advancements

VS Code’s September 2025 update (v1.105) carries forward agentic automation and collaborative MCP workflows from last week. In-editor AI-powered merge conflict resolution uses chat agents, advancing previous source control features.

Chain-of-thought debugging and improved session management for Copilot Chat enhance transparency and explainability, responding to earlier developer feedback. BYOK model previews add workflow flexibility, continuing registry and protocol expansion.

The MCP marketplace preview in Extensions follows registry improvements, making MCP server discovery easier. Customization and agent development become more central as protocol use expands.

Shell and terminal configuration updates, pull request enhancements, and agent-driven validation integrate with core test tooling—in line with past agentic workflow updates. These changes reinforce VS Code’s community-based, developer-driven evolution.

- [Visual Studio Code September 2025 Release (v1.105): AI, MCP, and Developer Enhancements](https://code.visualstudio.com/updates/v1_105)

### Application Performance and Stability: .NET 10 GC and WPF Troubleshooting

Prepping for .NET 10, developers get a deep dive into DATAS GC, which adapts heap sizing for real memory use—targeting containers, Kubernetes, and web apps. Configuration, performance tuning, and diagnostic instructions support safe migration.

For WPF app unresponsiveness with USB pen devices, guidance recommends disabling stylus/touch handlers using an AppContext switch; apps that require these features will need other fixes. Microsoft’s findings point to memory and deadlock issues, with a permanent solution yet to arrive.

- [Preparing for the .NET 10 GC: Understanding and Tuning DATAS](https://devblogs.microsoft.com/dotnet/preparing-for-dotnet-10-gc/)
- [WPF Apps Becoming Unresponsive after USB Pen Device Hotplug](https://techcommunity.microsoft.com/t5/app-development/wpf-application-becomes-unresponsive-after-plugging-unplugging/m-p/4459751#M1274)

### Open Source Licensing Guidance for .NET Foundation Projects

The .NET Foundation clarified its licensing guide, stating only permissive OSI licenses (MIT, Apache 2.0, BSD, ISC) are allowed for main code and dependencies. Copyleft (GPL, AGPL, RPL) is not accepted to avoid issues in commercial applications. The document discusses project governance, corrects myths, and lists monetization options, citing AutoMapper as a past example. Maintainers and teams can use compliance checks and scenario reviews in policy updates for sustainable open source projects.

- [.NET Foundation License Compatibility Guide](https://dotnetfoundation.org/news-events/detail/license-compatibility-guide)',
    'Coding news this week centers on practical updates for developer workflows, including platform releases, debugging, open source compliance, AI-supported code assistance, productivity tools, improved .NET memory management, and guidance for licensing in long-term open source projects.',
    1760346000, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2025-10-13', 'TechHub',
    'TechHub', '38801fd215070be2e857c2a1d130ce6b', ',AI Agents,Azure AI,CI/CD,Cloud Infrastructure,Data Engineering,Developer Tools,Multimodal Models,Observability,Open Source,Serverless Computing,Zero Trust,AI,GitHub Copilot,ML,Azure,DevOps,Security,Roundups,.NET,',
    false, false, true,
    false, false,
    false, false, 4, NULL
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2025-10-06
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2025-10-06', 'roundups', 'Agentic AI, Copilot Updates, and Platform Integration Drive Tech Ecosystem Developments',
    'Coding updates this week highlight improvements in Visual Studio and .NET, emphasizing better performance, day-to-day workflow enhancements, and clear API design. New frameworks and previews focus on more efficient, maintainable solutions for modern development.

<!--excerpt_end-->

## Coding

### Visual Studio 2026 Insiders: TypeScript 7 Native Preview and Razor Tooling Evolution

Visual Studio 2026 Insiders now offers a native preview for TypeScript 7, following last week’s advances in container tools and Aspire tracing. Native implementation yields faster compile times and reduced memory usage—empirical data from large projects (VS Code, Playwright) illustrates these gains. Microsoft is asking for community feedback to guide further changes.

Razor tooling updates improve Hot Reload and editing within the ASP.NET Community Standup, reflecting ongoing efforts for more responsive development and real-time feedback, with fewer build interruptions—continuing the consistent expansion of Visual Studio features.

- [TypeScript 7 Native Preview Now Available in Visual Studio 2026 Insiders](https://devblogs.microsoft.com/blog/typescript-7-native-preview-in-visual-studio-2026)
- [ASP.NET Community Standup - Razor Reloaded](/coding/videos/aspnet-community-standup-razor-reloaded)

### Coding Agents and Workflow Automation in Visual Studio Code

Coding agent integration in VS Code is further refined to improve speed and accuracy, building on last week’s MCP-based automation and collaborative development advances. Feedback mechanisms allow developers to directly influence future improvements, promoting real-world, robust workflow automation.

- [Latest Updates to Coding Agent Integration in Visual Studio Code](/coding/videos/latest-updates-to-coding-agent-integration-in-visual-studio-code)

### Modern ASP.NET API Architecture with FastEndpoints and the REPR Pattern

Guidance for ASP.NET API design now favors the REPR (Request, Endpoint, Response) pattern with FastEndpoints, stepping away from the older controller structure featured last week. Marcel Medina’s tutorial provides clear, testable examples that support migration and database management, focusing on maintainable, scalable solutions.

- [Simplifying ASP.NET API Design with the REPR Pattern and FastEndpoints](/coding/videos/simplifying-aspnet-api-design-with-the-repr-pattern-and-fastendpoints)

### Other Coding News

A timely reminder steers developers toward maximizing Visual Studio subscription perks, including monthly Azure credits, access to Microsoft dev/test software, and training portals such as Pluralsight and Cloud Academy. These tips support ongoing .NET lifecycle and migration planning for more efficient workflows.

- [Unlocking the Hidden Value of Your Visual Studio Subscription](https://devblogs.microsoft.com/visualstudio/unlock-vss-benefits-myvisualstudio/)',
    'Coding updates this week highlight improvements in Visual Studio and .NET, emphasizing better performance, day-to-day workflow enhancements, and clear API design. New frameworks and previews focus on more efficient, maintainable solutions for modern development.',
    1759741200, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2025-10-06', 'TechHub',
    'TechHub', '36a4117a842c55468f34233590a51127', ',AI Agents,Automation,Coding Agents,Data Transformation,Enterprise Cloud,MCP,Microsoft Fabric,Multicloud,VS,Workflow Orchestration,AI,GitHub Copilot,ML,Azure,DevOps,Security,Roundups,.NET,',
    false, false, true,
    false, false,
    false, false, 4, NULL
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2025-09-29
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2025-09-29', 'roundups', 'GitHub Copilot, Azure AI, and DevOps: Updates on Agentic Automation and Cloud Workflows',
    'This week’s coding highlights include updates in .NET development, new container tooling in Visual Studio, and practical advice on platform compliance, distributed workflows, and migration planning.

<!--excerpt_end-->

## Coding

### Visual Studio 2026 and Container Tooling

Visual Studio 2026 Insiders now supports Podman, enabling developers to use this daemonless, rootless container engine instead of Docker for increased security and flexibility. The IDE detects Podman automatically and offers tools for managing images, debugging, and working with containers from the terminal—making secure Linux container development more approachable.

- [Visual Studio 2026 Insiders: Using Podman for Container Development](https://devblogs.microsoft.com/blog/visual-studio-2026-insiders-using-podman-for-container-development)

### .NET Aspire 9.5 and Modern .NET Cloud-Native Development

.NET Aspire 9.5 provides improvements for distributed .NET applications, including a new ''aspire update'' CLI for managing SDK/package upgrades, improved dashboards, a single-file AppHost preview for fast prototyping, and color-coded telemetry. GenAI Visualizer aids model debugging, YARP supports static files, and integration with Azure DevTunnels supports local secure testing. Visual Studio 2026 picks up new Aspire tracing features, and migration guides offer help for upgrades from Aspire 8.x.

- [Announcing Aspire 9.5](https://devblogs.microsoft.com/dotnet/announcing-dotnet-aspire-95/)

### .NET MAUI: App Compliance, Migration, and Community Engagement

.NET MAUI applications must update to MAUI 9 to comply with Google Play''s 16 KB memory page rule for Android 15+. Guidance is available for checking dependencies and updating build tools. The MAUI Community Standup event in Prague continues focus on collaboration and ongoing platform improvements, reflecting recent compliance and migration support topics.

- [Preparing Your .NET MAUI Apps for Google Play’s 16 KB Page Size Requirement](https://devblogs.microsoft.com/dotnet/maui-google-play-16-kb-page-size-support/)
- [.NET MAUI Community Standup - Live in Prague with the .NET MAUI Team](/coding/videos/net-maui-community-standup-live-in-prague-with-the-net-maui-team)

### .NET Platform Strategy and Database Migrations

Microsoft has clarified support timelines for .NET LTS/STS releases. Nick Chapsas provides migration planning guidance, helping developers minimize upgrade risk. Jeremy Miller’s Data Community Standup compares Marten/PostgreSQL and Entity Framework Core, offering real-world migration Q&A for developers planning database changes.

- [Understanding Microsoft''s LTS/STS Changes for .NET Support](/coding/videos/understanding-microsofts-ltssts-changes-for-net-support)
- [.NET Data Community Standup: Jeremy Miller on Marten and Database Migrations](/coding/videos/net-data-community-standup-jeremy-miller-on-marten-and-database-migrations)

### Building Server-Side and CLI Tools with .NET

The latest ASP.NET Community Standup demonstrates a multi-user MCP server, highlighting collaborative code review and refactoring workflows. Andrew Lock’s guide on ''sleep-pc'' covers .NET Native AOT usage, Win32 integration, argument processing, and NuGet packaging for durable server-side and CLI tool creation.

- [ASP.NET Community Standup - Vibe Coding a C# MCP Server](/coding/videos/aspnet-community-standup-vibe-coding-a-c-mcp-server)
- [Building sleep-pc: A .NET Native AOT Tool for Automating Windows Sleep](https://andrewlock.net/sleep-pc-a-dotnet-tool-to-make-windows-sleep-after-a-timeout/)',
    'This week’s coding highlights include updates in .NET development, new container tooling in Visual Studio, and practical advice on platform compliance, distributed workflows, and migration planning.',
    1759136400, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2025-09-29', 'TechHub',
    'TechHub', '0d6531e1439888068cca1ac0dd29ec35', ',.NET,Agentic Automation,AI Agents,Azure AI,Claude Opus,Cloud Modernization,Containerization,Hybrid Cloud,IaC,MCP,Microsoft Fabric,MLOps,OpenAI,Supply Chain Security,VS,AI,GitHub Copilot,ML,Azure,DevOps,Security,Roundups,',
    false, false, true,
    false, false,
    false, false, 4, NULL
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2025-09-22
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2025-09-22', 'roundups', 'AI Agent Workflows, Security Improvements, and Advances in Cloud Technology',
    'Coding updates this week focus on practical improvements for .NET developers, tool packaging strategies, performance, and changes to support life cycles. As .NET 10 nears release, content covers actionable planning advice for evolving within the .NET ecosystem.

<!--excerpt_end-->

## Coding

### .NET 10 Tool Packaging, WebAssembly, and Support Lifecycle Changes

Andrew Lock’s expanded coverage from last week’s .NET 10 RC1 preview addresses platform-specific tool packaging, new schemas for DotnetToolSettings.xml, and approaches for dual packaging to maintain support for older SDKs, as illustrated by Datadog and NativeAOT examples—continuing migration support started earlier.

The latest ASP.NET Community Standup covers .NET 10’s WebAssembly runtime, outlining performance enhancements and migration tactics that build upon last week’s component feature improvements.

Microsoft will extend .NET Standard Term Support to 24 months beginning with .NET 9, directly addressing prior migration challenges and adopting past recommendations for update planning.

- [Supporting Platform-Specific .NET Tools on Older SDKs: .NET 10 Preview Deep Dive](https://andrewlock.net/exploring-dotnet-10-preview-features-8-supporting-platform-specific-dotnet-tools-on-old-sdks/)
- [ASP.NET Community Standup: .NET 10 WebAssembly Performance Enhancements](/coding/videos/aspnet-community-standup-net-10-webassembly-performance-enhancements)
- [.NET Standard Term Support (STS) Releases Will Be Supported for 24 Months Starting with .NET 9](https://devblogs.microsoft.com/dotnet/dotnet-sts-releases-supported-for-24-months/)',
    'Coding updates this week focus on practical improvements for .NET developers, tool packaging strategies, performance, and changes to support life cycles. As .NET 10 nears release, content covers actionable planning advice for evolving within the .NET ecosystem.',
    1758531600, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2025-09-22', 'TechHub',
    'TechHub', '488abaa6f857ae2086b65d4b574a4e20', ',AI Agents,Application Modernization,Automation,Cloud Migration,Developer Tools,Kubernetes,LLM,MCP,Microsoft Fabric,Quantum Computing,AI,GitHub Copilot,ML,Azure,DevOps,Security,Roundups,.NET,',
    false, false, true,
    false, false,
    false, false, 4, NULL
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2025-09-15
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2025-09-15', 'roundups', 'Updates in AI Coding, Azure Features, and Secure DevOps for Modern Teams',
    'This section highlights new milestone releases and tooling updates from Microsoft. With .NET 10 approaching general release, developers are encouraged to start testing, while Visual Studio and VS Code both see new AI and automation features. Guides offer practical advice for modern development and migration.

<!--excerpt_end-->

## Coding

### .NET 10 Release Candidate 1 and Ecosystem Updates

.NET 10 RC1 is available for production testing. Previous updates focused on CLI improvements, multi-platform workflows, and new language features—now, RC1 can be tried with Visual Studio 2026 Insiders and VS Code’s C# Dev Kit. Updates include new quantum-safe cryptography APIs, faster UTF-8 hex-string conversions, and new tensor types for numerical computation. Runtime, SDK, and languages (C#, F#, VB) have refinements for increased stability.

ASP.NET Core features new Blazor component state persistence, expanded identity support, improved Minimal API validation, and better OpenAPI docs. Blazor continues UX and platform improvements, while .NET MAUI offers debugging, new UI events, enhanced refresh controls, and early CoreCLR support on Android. Windows Forms now has dark mode, improved themes, async capabilities, and easier renderer management. Entity Framework Core adds vector search in SQL Server, native JSON types, enhanced Cosmos DB support, and better complex type handling.

Developers are encouraged to validate their applications on RC1, check for breaking changes, join standups, and plan migrations. The .NET Unboxed event shares rollout details and technical Q&A.

- [Announcing .NET 10 Release Candidate 1](https://devblogs.microsoft.com/dotnet/dotnet-10-rc-1/)
- [.NET Unboxed - .NET 10 Release Candidate 1](/coding/videos/net-unboxed-net-10-release-candidate-1)

### Visual Studio 2026 Insiders and VS Code v1.104 Feature Enhancements

Visual Studio 2026 Insiders is a new early-access program for monthly IDE updates and direct community feedback. AI integration improves code completion, automated testing, and review—embeddeding GitHub Copilot Free for all users. Enhanced solution management, Profiler Agent, and faster performance are available for x64/Arm64. UI updates include new Fluent designs, themes, and user onboarding, shaped by user contributions.

VS Code v1.104 features Agent Mode for workspace automation—allowing developers to offload routine tasks and focus on coding. Additional updates include improved APIs for plugins, automated terminal approval, and new TODO management. Git Worktree support simplifies multi-branch reviews and development, enhancing last week’s prompt automation for collaboration.

- [Visual Studio 2026 Insiders Launch: Integrated AI, Performance, and Developer-Centric Upgrades](https://devblogs.microsoft.com/visualstudio/visual-studio-2026-insiders-is-here/)
- [Latest Features in Visual Studio Code: Agent Mode, Git Worktrees, and More](/ai/videos/latest-features-in-visual-studio-code-agent-mode-git-worktrees-and-more)
- [VS Code Live: Exploring Hidden Features in VS Code v1.104](/coding/videos/vs-code-live-exploring-hidden-features-in-vs-code-v1104)

### .NET Tooling, Packaging, and Servicing Releases

A new guide shows how .NET 10 enables expanded packaging for multi-targeted, reusable tools—developers can create NuGet packages for self-contained, trimmed, or ahead-of-time compiled tool distributions. The article includes deployment and configuration samples and recommends thorough testing ahead of general release.

Routine servicing for .NET 8 and .NET 9 in September 2025 brings bug fixes and reliability updates for ASP.NET Core, the SDK, and .NET Framework across all platforms. Developers should consult changelogs and apply fixes to maintain platform stability.

- [Packaging Self-Contained and Native AOT .NET Tools for NuGet: .NET 10 Preview](https://andrewlock.net/exploring-dotnet-10-preview-features-7-packaging-self-contained-and-native-aot-dotnet-tools-for-nuget/)
- [.NET and .NET Framework September 2025 Servicing Releases Updates](https://devblogs.microsoft.com/dotnet/dotnet-and-dotnet-framework-september-2025-servicing-updates/)

### Data Access Strategies: Dapper vs Entity Framework Core

The .NET Data Community Standup compared Dapper and Entity Framework Core. Presenters shared concrete lessons—Dapper’s speed and simplicity versus EF Core’s feature set and maintainability. The session covered use cases, performance tips, pitfalls, and factors for teams deciding between direct SQL mapping and a higher-level ORM.

- [.NET Data Community Standup: Practical Dapper vs Entity Framework Core Comparison](/coding/videos/net-data-community-standup-practical-dapper-vs-entity-framework-core-comparison)',
    'This section highlights new milestone releases and tooling updates from Microsoft. With .NET 10 approaching general release, developers are encouraged to start testing, while Visual Studio and VS Code both see new AI and automation features. Guides offer practical advice for modern development and migration.',
    1757926800, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2025-09-15', 'TechHub',
    'TechHub', 'c1d430208e26d1b6f92a026904f5750a', ',AI Coding,AI Infrastructure,CI/CD,Cloud Computing,Data Management,Enterprise Apps,Low Code Automation,Observability,Open Source,VS Code,AI,GitHub Copilot,ML,Azure,DevOps,Security,Roundups,.NET,',
    false, false, true,
    false, false,
    false, false, 4, NULL
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2025-09-08
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'Latest Updates in AI Development, Cloud Automation, and Enterprise Security',
    'This week features guides and platform enhancements in .NET, Blazor, .NET MAUI, and VS Code. Developers have new resources for building .NET CLI tools, web and mobile apps, and more secure and automated workflows. Further MCP and AI improvements in VS Code support efficient, streamlined daily development.

<!--excerpt_end-->

## Coding

### .NET Ecosystem: Tooling, Web, and Cross-Platform Updates

Continuing last week’s exploration of .NET tooling, Andrew Lock’s article covers how to create robust .NET tools, including tips on global vs. local installations, repeatable manifests, multi-targeting for compatibility, RollForward for stable versioning, and integration with CI/test automation. Developers will learn how to set up tools efficiently, manage versions, and avoid common CI issues.

The Blazor Internship Demo Fest highlights a new media component and improved navigation state, simplifying multimedia usage and app navigation in Blazor. These insights reinforce best practices in UI design and community engagement.

.NET MAUI’s Community Standup introduces Release Candidate 1, improved iPhone support, and new cross-platform features, with practical recommendations for mobile app delivery. Q&A and hands-on content help developers adapt to changing mobile targets—building on prior updates to release cycles and architectural patterns.

- [Using and Authoring .NET Tools: Multi-Targeting, CI, and Best Practices](https://andrewlock.net/using-and-authoring-dotnet-tools/)
- [Blazor Internship Demo Fest: New Components and Enhanced Navigation](/coding/videos/blazor-internship-demo-fest-new-components-and-enhanced-navigation)
- [.NET MAUI Community Standup: Release Candidates, iPhone Support, and Updates](/coding/videos/net-maui-community-standup-release-candidates-iphone-support-and-updates)

### VS Code and MCP: Workflow Automation and AI Integration

Reflecting last week’s work with MCP and Playwright, VS Code now has native MCP server support, allowing for secure authentication and automation of end-to-end tests. Developers are able to run protocol automation directly from their IDE alongside traditional code and CI activities. The integration with Playwright and GitHub MCP further enhances real-world automation.

Kent C. Dodds explains how automation, including authentication to AI coding support, fits into everyday development—demonstrating the shift from initial protocol configuration to stable production usage.

- [Building an MCP Inside VS Code and Exploring AI''s Impact with Kent C. Dodds](/ai/videos/building-an-mcp-inside-vs-code-and-exploring-ais-impact-with-kent-c-dodds)',
    'This week features guides and platform enhancements in .NET, Blazor, .NET MAUI, and VS Code. Developers have new resources for building .NET CLI tools, web and mobile apps, and more secure and automated workflows. Further MCP and AI improvements in VS Code support efficient, streamlined daily development.',
    1757322000, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2025-09-08', 'TechHub',
    'TechHub', '668eb5f8cca416da52915b53b193e439', ',.NET,AI Agents,Azure AI,Blazor,CI/CD,Cloud Infrastructure,Code Review,Compliance,Data Analytics,Developer Tooling,Enterprise Security,Identity Management,Kubernetes,MCP,Microsoft Fabric,OpenAI,VS Code,Workflow Automation,AI,GitHub Copilot,ML,Azure,DevOps,Security,Roundups,',
    false, false, true,
    false, false,
    false, false, 4, NULL
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2025-09-01
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2025-09-01', 'roundups', 'Updates in AI Developer Tools, Secure Cloud Features, and DevOps Practices',
    'Updates this week for coding include new features in .NET and C#, diagnostic tools, open sourcing of Windows Subsystem for Linux, and practical workflow guides. Microsoft continues its focus on open source and improving developer experience with new releases and troubleshooting content.

<!--excerpt_end-->

## Coding

### .NET and C# Language and Platform Enhancements

Nick Chapsas previews Discriminated Unions for C# 15/16, demonstrating better type safety and simplified code patterns akin to F#, TypeScript, and Rust. MauiReactor provides MVU architecture options for .NET MAUI UI development. EFCore.Visualizer lets Visual Studio users analyze Entity Framework Core query plans inside the IDE, continuing improvements in .NET tooling.

- [Exploring Discriminated Unions Coming to C# 15 and 16](/coding/videos/exploring-discriminated-unions-coming-to-c-15-and-16)
- [MauiReactor: Introducing the MVU Pattern for .NET MAUI](https://devblogs.microsoft.com/dotnet/mauireactor-mvu-for-dotnet-maui/)
- [EFCore.Visualizer: Analyze Entity Framework Core Query Plans in Visual Studio](https://devblogs.microsoft.com/dotnet/ef-core-visualizer-view-entity-framework-core-query-plan-inside-visual-studio/)

### Open Sourcing Windows Subsystem for Linux (WSL)

Microsoft has published the source code for WSL internals—including VM startup, filesystem mounting, and GPU handling—on GitHub, boosting community involvement and transparency. Developers have direct access for troubleshooting, customize workflow, and can contribute ideas for enhancements or fixes. Official resources welcome community collaboration and learning.

- [You open sourced WSL. What does that mean?](/coding/videos/you-open-sourced-wsl-what-does-that-mean)
- [Explaining the Open Sourcing of Windows Subsystem for Linux (WSL)](/coding/videos/explaining-the-open-sourcing-of-windows-subsystem-for-linux-wsl)

### Developer Experience and Workflow Tools

Aspire CLI for .NET streamlines app configuration, integrates cloud dependencies, and supports easy deployment to Azure, Docker Compose, or Kubernetes. Step-by-step guides for installation and commands aim to make distributed development more approachable. VS Code’s August iteration plan previews new terminal enhancements, agent features, and automation, opening discussion for community feedback.

- [Getting Started with the Aspire CLI](https://devblogs.microsoft.com/dotnet/getting-started-with-the-aspire-cli/)
- [Upcoming Features and Terminal Improvements in Visual Studio Code: August Iteration](/coding/videos/upcoming-features-and-terminal-improvements-in-visual-studio-code-august-iteration)

### Other Coding News

A troubleshooting guide for .NET Core on Alpine Linux explains fixes for native asset loading errors, including environment variable recommendations. A SharePoint branding resource provides practical steps for logo setup, themes, templates, and homepage configuration for consistent organizational branding.

- [Solving Native Library Loading Issues for .NET Core on Alpine Linux](https://andrewlock.net/fixing-an-old-dotnet-core-native-library-loading-issue-on-alpine/)
- [Branding Your SharePoint Site for Your Organization](https://dellenny.com/branding-your-sharepoint-site-for-your-organization/)',
    'Updates this week for coding include new features in .NET and C#, diagnostic tools, open sourcing of Windows Subsystem for Linux, and practical workflow guides. Microsoft continues its focus on open source and improving developer experience with new releases and troubleshooting content.',
    1756717200, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2025-09-01', 'TechHub',
    'TechHub', 'd7d21bc0c925e9a656d6bf13cf31d43e', ',.NET,AI Agents,Cloud Infrastructure,Data Engineering,Developer Productivity,Kubernetes,LLM Benchmarking,Open Source,Platform Automation,Semantic Kernel,Supply Chain Security,AI,GitHub Copilot,ML,Azure,DevOps,Security,Roundups,',
    false, false, true,
    false, false,
    false, false, 4, NULL
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2025-08-25
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2025-08-25', 'roundups', 'Updated AI DevOps, Copilot Personalization, and Secure Cloud Developments',
    'Developers benefit this week from updated toolchains and workflow features, helping with modernization and streamlining in both Git and .NET. Git delivers new storage and workflow improvements, while .NET introduces new approaches for testing, migration, and UI challenges.

<!--excerpt_end-->

## Coding

### Git 2.51: Storage, Workflow, and CLI Enhancements

Git 2.51 delivers enhancements to storage and workflow efficiency. The new cruft-free MIDX feature allows for duplication of reachable objects from cruft packs, resulting in smaller, faster repositories and up to 38% storage reduction. This is managed via the `repack.MIDXMustContainCruft` setting. The updated `git repack --path-walk` uses file layout to optimize delta compression and pack size.

A new stash interchange format allows linked ancestor commits, enabling stash export/import across devices. Scripting is improved via more accurate `git cat-file` submodule reporting; commit-graph Bloom filters offer accelerated path filtering in large repositories.

The commands `git switch` and `git restore` are now stable, while the deprecated `git whatchanged` is replaced by `git log --raw`. Looking ahead, Git 3.0 will default to SHA-256 and implement a new reftable backend, so users should start planning for migration. Updates to C99 support and patch submission workflows further modernize the codebase.

These updates maintain Git’s focus on workflow effectiveness and code modernization.

- [Key Updates in Git 2.51: Cruft-Free MIDX, Stash Interchange, and More](https://github.blog/open-source/git/highlights-from-git-2-51/)

### .NET Testing Modernization: CLI, TUnit Migration, and Email Workflow

.NET 10 now runs `dotnet test` using the Microsoft.Testing.Platform (MTP), replacing the previous VSTest engine. The update improves automation, performance, diagnostics, filtering, parallelism, and output. Developers should migrate tests to MTP, update configurations, and remove obsolete settings for simpler and faster solution builds.

Migration resources confirm that moving from xUnit to TUnit is direct, with analyzer and source generator support for parallel, NativeAOT-ready, and .NET Standard 2.0 projects. Guides cover assert conversion, snapshot usage, and CI integration.

A new tutorial explains how to implement reliable email sending within .NET, covering SMTP setup, formatting, debugging, and best practices for maintainable code.

- [Enhance your CLI testing workflow with the new dotnet test](https://devblogs.microsoft.com/dotnet/dotnet-test-with-mtp/)
- [Migrating an xUnit Test Project to TUnit: Experience, Issues, and Practical Steps](https://andrewlock.net/converting-an-xunit-project-to-tunit/)
- [Sending Email Correctly in .NET](/coding/videos/sending-email-correctly-in-net)

### .NET Application Modernization: Migration, WebView2, and Obsolete APIs

Migrating .NET Framework 4.8 applications to .NET 8 is now easier, with tools like Upgrade Assistant, Portability Analyzer, and Roslyn analyzers designed for incremental and batch migration. Documentation and automation help prioritize modernization and minimize risk.

These resources support ongoing themes around cross-platform modernization and multi-targeted project upgrades, including those for Aspire and MAUI. The focus is on easy migration of APIs and structuring large solution upgrades.

WebView2 now supports improved keyboard input mapping, simplifying desktop usability in WPF/WinForms applications. The new `CoreWebView2ControllerOptions.AllowHostInputProcessing` property restores expected keyboard behaviors, supporting better integration between host and browser-based UIs.

Additional tools are available for mapping obsolete APIs to .NET 8. This continues last week’s theme of smoother API and UI modernization.

- [Handling Keyboard Mapping in WebView2 with AllowHostInputProcessing](https://weblog.west-wind.com/posts/2025/Aug/20/Using-the-new-WebView2-AllowHostInputProcessing-Keyboard-Mapping-Feature)
- [Tools and Approaches for Migrating Obsolete .NET Framework APIs to .NET 8](https://techcommunity.microsoft.com/t5/tools/tool-or-approach-to-identify-and-replace-obsolete-net-framework/m-p/4446845#M161)',
    'Developers benefit this week from updated toolchains and workflow features, helping with modernization and streamlining in both Git and .NET. Git delivers new storage and workflow improvements, while .NET introduces new approaches for testing, migration, and UI challenges.',
    1756112400, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2025-08-25', 'TechHub',
    'TechHub', '1c4d237dbf31c3a38d721193249280eb', ',.NET Modernization,AI Agents,Azure Updates,CI/CD Pipelines,Cloud Infrastructure,Developer Productivity,DevOps Automation,Enterprise Security,Git,MCP,ML Optimization,Quantum Safe Security,VS,Workflow Automation,AI,GitHub Copilot,ML,Azure,DevOps,Security,Roundups,.NET,',
    false, false, true,
    false, false,
    false, false, 4, NULL
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2025-08-18
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', 'This Week’s Key Developments in GitHub Copilot, Azure, and Cloud',
    'Developers are getting meaningful updates this week, from fresh language features in .NET 10 Preview and smarter Excel Python tools to improved resilience in Spark. New guides cover everything from disk cleanup to building dual-transport servers, reflecting a bigger focus on practical, modern, and cross-platform workflows.

<!--excerpt_end-->

## Coding

### Major Platform Features and Updates: .NET 10 Preview, Excel Python Image Analysis, Spark Resilience

.NET 10 Preview 7 brings new cryptographic features, faster JSON serialization, better diagnostics, easier authentication, and stronger cross-platform support. The improvements boost usability, security, and help streamline cloud-native workflows.

The latest Excel update lets you use Python natively for image analysis tasks like blur detection, brightness checks, and metadata collection—bringing advanced vision analysis tools to everyday spreadsheet users.

Spark’s improved Iteration Panel makes file filtering and API failure handling easier, helping with smoother, more reliable development cycles.

- [.NET 10 Preview 7 Released: Key Updates for Libraries, ASP.NET Core, Blazor, and MAUI](https://devblogs.microsoft.com/dotnet/dotnet-10-preview-7/)
- [Analyzing Images with Python in Excel: Now Natively Supported](https://techcommunity.microsoft.com/t5/microsoft-365-insider-blog/analyze-images-with-python-in-excel/ba-p/4440388)
- [Spark Resilience Improvements Enhance Reliability and Iteration Experience](https://github.blog/changelog/2025-08-13-spark-resilience-improvements)

### Guides for Modern, Cross-Platform Development: .NET Aspire, .NET MAUI, Browser-Based .NET

Step-by-step guides help teams get started with .NET Aspire for distributed systems, including boilerplate code, GitHub Actions integration, and custom metrics for ongoing monitoring. Visual Studio and .NET MAUI tutorials focus on building cross-platform mobile and desktop apps—with advice on UI optimization, file size reduction, and streamlined updates.

A walkthrough from Andrew Lock shows how to run .NET in the browser without Blazor, using WebAssembly templates and JavaScript interop for high-performance client apps.

- [Building Confident Application Systems with .NET Aspire: From Dev to Deployment](/azure/videos/building-confident-application-systems-with-net-aspire-from-dev-to-deployment)
- [Building Mobile and Desktop Apps with Visual Studio and .NET MAUI](/coding/videos/building-mobile-and-desktop-apps-with-visual-studio-and-net-maui)
- [Running .NET in the Browser Without Blazor Using WASM](https://andrewlock.net/running-dotnet-in-the-browser-without-blazor/)

### Language Evolution and the Future of Web Development: C# 14, ASP.NET Core & Blazor in .NET 10

A detailed look at C# 14 covers improved pattern matching, nullability support, and value types—making code safer and cleaner. Current .NET team previews for ASP.NET Core and Blazor in .NET 10 include modern security, diagnostics, WebAuthn support, integrated AI libraries, and faster project ramp-up with Aspire, helping developers stay current with critical web advances.

- [Highlights and Upcoming Features in C#: A Deep Dive into C# 14](/coding/videos/highlights-and-upcoming-features-in-c-a-deep-dive-into-c-14)
- [The Future of Web Development with ASP.NET Core & Blazor in .NET 10](/ai/videos/the-future-of-web-development-with-aspnet-core-and-blazor-in-net-10)

### Other Coding News

VS Code’s “Beast mode” rolls out improvements for batch edits, UI adjustments, and workflow enhancements, with a video guide to help users get started. Additional tutorials show how to simplify .NET mapping with Facet, automate disk cleanup with PowerShell, and build STDIO/HTTP dual-transport MCP servers for flexible cloud and local deployments.

- [VS Code Beast Mode Explained: Features and Usage](/coding/videos/vs-code-beast-mode-explained-features-and-usage)
- [Enhancing .NET Code: Using Facet Instead of Traditional Mapping](/coding/videos/enhancing-net-code-using-facet-instead-of-traditional-mapping)
- [Finding Large Directories and Recovering Lost Disk Space with PowerShell](https://techcommunity.microsoft.com/t5/windows-powershell/how-to-finding-large-directories-recovering-lost-space/m-p/4442877#M9117)
- [Building a Dual-Transport MCP Server with .NET: STDIO and HTTP Support](https://techcommunity.microsoft.com/t5/microsoft-developer-community/one-mcp-server-two-transports-stdio-and-http/ba-p/4443915)',
    'Developers are getting meaningful updates this week, from fresh language features in .NET 10 Preview and smarter Excel Python tools to improved resilience in Spark. New guides cover everything from disk cleanup to building dual-transport servers, reflecting a bigger focus on practical, modern, and cross-platform workflows.',
    1755507600, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2025-08-18', 'TechHub',
    'TechHub', '6b5737660766e82651ce6730cdaac62d', ',.NET,Agentic AI,AI Tools,Application Security,Cloud Security,Copilot Studio,Data Analytics,Enterprise Automation,GPT 5,LLMs,MCP,Microsoft Fabric,Open Source,VS Code,AI,GitHub Copilot,ML,Azure,DevOps,Security,Roundups,',
    false, false, true,
    false, false,
    false, false, 4, NULL
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2025-08-11
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2025-08-11', 'roundups', 'AI Agents and Automation Redefine Developer Workflows, Security, and Cloud Operations',
    'This week’s .NET ecosystem saw major advances in authentication, language expressiveness, cloud-native tooling, and real-world developer productivity.

<!--excerpt_end-->

## Coding

### Cloud-Native ASP.NET Core and Passwordless Authentication Advance

.NET 10 Preview 6 brings passkey authentication support to ASP.NET Core and Blazor. Aspire—a toolkit for cloud-native app enablement—makes distributed app development more approachable with rich documentation, tooling, and CI/CD support.

- [Exploring Passkey Support in ASP.NET Core Identity with .NET 10 Preview 6](https://andrewlock.net/exploring-dotnet-10-preview-features-6-passkey-support-for-aspnetcore-identity/)
- [ASP.NET Community Standup: Why ASP.NET Core Developers Will Love Aspire](/coding/videos/aspnet-community-standup-why-aspnet-core-developers-will-love-aspire)

### Language Innovations: C# 14 Extension Members and Future Nominal Union Types

C# 14 introduces “Extension Everything”—allowing methods, properties, and operators on existing types. Upcoming nominal union types are poised to simplify modeling alternatives in future C#, driven by deep community interest over several weeks.

- [C# 14 Extension Members: Also Known as Extension Everything - NDepend Blog](https://www.reddit.com/r/dotnet/comments/1mhbukq/c_14_extension_members_also_known_as_extension/)
- [Discussion: Nominal Union Types Demoed at VS Live, Redmond](https://www.reddit.com/r/csharp/comments/1mj96yf/nominal_union_types_were_demoted_at_vs_live_at/)

### Practical Tools and Productivity Boosts Across the Stack

A unified .NET CLI tool is in the works for SDK/runtime management. Community reviews focus on code formatting for old .NET versions and powerful terminal file managers like Termix v1.2.0. The VS Code 1.103 release targets common C#/VS Code “papercuts,” complementing recent .NET tooling expansions.

- [Discussion: New CLI Tool for .NET SDK Management and Updates](https://www.reddit.com/r/dotnet/comments/1mjgvq3/want_to_make_it_easier_to_get_startedstay_up_to/)
- [Termix v1.2.0: .NET Terminal File Manager Adds Cut, Copy, Move, and Stable Fuzzy Search](https://www.reddit.com/r/dotnet/comments/1mhgcod/beautiful_terminal_based_file_manager_now/)

### Testing, Orchestration, and Advanced Workflows

TUnit testing framework now orchestrates parallel dependency injection and resource sharing for integration tests. Detailed guides tackle deterministic cleanup with IAsyncDisposable, config loading post-OS patching, and diagnosing high RAM use in ASP.NET Core apps.

- [TUnit Test Orchestration: Advanced Setup and Parallel Dependency Injection](https://www.reddit.com/r/csharp/comments/1mjgiuq/tunit_test_orchestration/)
- [High RAM Usage Troubleshooting in ASP.NET Core MVC 8 Application](https://www.reddit.com/r/dotnet/comments/1mk4hp1/high_ram_usage_aspnet_core_mvc/)

### Architectures, Patterns, and API Modeling Debates

Debates abound on modular monoliths, repository vs. CQRS, and navigation property best practices. New source generators and reflective case studies surface practical strategies for large codebases, echoing last week’s focus on maintainability and architecture.

- [Reflections on .NET Project Structure and Complexity for Beginners](https://www.reddit.com/r/dotnet/comments/1mkujgo/starting_to_understand_the_differences_of_dotnet/)

### Ecosystem: Updates, Conferences, and the Road Ahead

Microsoft opened the .NET Conf 2025 call for content. Rx.NET modernizes its packaging, and servicing updates for .NET 8/9/Framework ensure stability. These reinforce the open and evolving ecosystem trend observed last week.

- [.NET Conf 2025 Call for Content: Share Your .NET 10 Expertise](https://devblogs.microsoft.com/dotnet/dotnet-conf-2025-announcing-the-call-for-content/)

### Efficiency, Low-Level Performance, and Real-World Coding Tactics

Span-based ZaString delivers zero-allocation string building. JIT vs. AOT benchmarks inform performance optimizations, while community tactics address cross-platform processor detection, deadletter queue processing, and efficient helper methods.

- [ZaString: A Zero-Allocation Span-Based String Builder for .NET](https://www.reddit.com/r/dotnet/comments/1mkqa37/stop_allocating_strings_i_built_a_spanpowered/)

### Learning, Community Growth, and Real-World Case Studies

Upskilling, .NET migration, SCADA scripting, and effective LINQ remain focal points, infusing the community with resources and support for every career stage.

- [Transitioning from .NET Framework 4.8 to Modern .NET (Core/9): Advice & Resources](https://www.reddit.com/r/dotnet/comments/1mk1z6x/studying_net_coming_from_net_framework/)',
    'This week’s .NET ecosystem saw major advances in authentication, language expressiveness, cloud-native tooling, and real-world developer productivity.',
    1754902800, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2025-08-11', 'TechHub',
    'TechHub', '06bd163d47e12948de5e15ccd5a13be9', ',.NET,AI Agents,CI/CD,Claude Opus 4.1,Cloud Identity,Enterprise AI,GPT 5,MCP,Multi Agent Systems,Passwordless Authentication,VS Code,AI,GitHub Copilot,ML,Azure,DevOps,Security,Roundups,',
    false, false, true,
    false, false,
    false, false, 4, NULL
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2025-08-04
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2025-08-04', 'roundups', 'Agentic AI and Developer Workflows Leap Forward',
    'This week brought strong advances to .NET Aspire, TypeScript, and C# tooling, deeper open-source integration, and community-driven productivity patterns. New releases and workflows point to a modern, flexible Microsoft developer stack supporting both rapid prototyping and scalable, production-grade delivery.

<!--excerpt_end-->

## Coding

### .NET Aspire Ecosystem: Distributed App Orchestration

Aspire 9.4 introduced a GA CLI with fast scaffolding, dashboard notifications, and AI model hosting support—simplifying distributed service orchestration and boosting workflow speed. New APIs make adding and monitoring external services easier, while the dashboard and hosting enhancements align Aspire with broader MCP-driven architectures.

- [Aspire 9.4: CLI, Interactive Dashboard, AI Integrations, and New Features](https://devblogs.microsoft.com/dotnet/announcing-aspire-9-4/)
- [.NET Aspire 9.4 New Features: ExternalService Resource, Interaction Service, Aspire CLI, and GitHub Model Integration](/coding/videos/net-aspire-94-new-features-externalservice-resource-interaction-service-aspire-cli-and-github-model-integration)
- [.NET Aspire – Adding Custom Commands to the Dashboard](/coding/videos/net-aspire-adding-custom-commands-to-the-dashboard)

### Full-Stack Development: Modern Patterns and Open Source

Integration tutorials for React with Aspire/ASP.NET Core APIs show how modern front/back workflows, DB migrations, and AI debugging now operate in an all-in-one, frictionless pipeline. Open-source stacks and templates (e.g., Xams) fuel collaborative modernization for .NET teams.

- [Building a Full-Stack App with React and Aspire: Step-by-Step Integration with ASP.NET Core Web API](https://devblogs.microsoft.com/dotnet/new-aspire-app-with-react/)
- [Open-sourced the ASP.NET + React Stack for Internal Business App Development](https://www.reddit.com/r/dotnet/comments/1mgk8cw/opensourced_the_aspnet_react_stack_we_use_to/)
- [Templates for MVC / Razor Pages with a Modern Frontend Build System](https://www.reddit.com/r/dotnet/comments/1mf10vc/templates_for_mvc_razor_pages_with_a_modern/)

### C# Language Evolution: Type Union Advancements

The C# language team adopted type union proposals, marking a move toward safer, more expressive APIs and code. This aligns with .NET’s ongoing shift to ergonomic, maintainable codebases and responds to long-standing community requests for language flexibility.

- [More Type Union Proposals Adopted by the C# Language Design Team](https://www.reddit.com/r/dotnet/comments/1mf2ylu/more_type_union_proposals_adopted_by_the_c/)
- [More type union proposals adopted by the language design team!](https://www.reddit.com/r/csharp/comments/1mf2xll/more_type_union_proposals_adopted_by_the_language/)
- [C# 15 Wishlist: What Features Do Developers Hope For?](https://www.reddit.com/r/csharp/comments/1meqqrk/c_15_wishlist/)

### TypeScript 5.9: JavaScript Tooling Modernization

TS 5.9 delivers streamlined config, ECMAScript import enhancements, improved DOM typings, stronger type inference, and speed boosts. This builds on last week’s RC and signals robust, forward-compatible JavaScript tooling for future upgrades.

- [Announcing TypeScript 5.9: New Features, Improvements, and What’s Next](https://devblogs.microsoft.com/typescript/announcing-typescript-5-9/)

### Expanding .NET and C# Tooling

.NET 10’s `dnx` CLI enables quick-use tools without installs; Aspire Event Hub Live Explorer improves event-driven debugging locally; open-source utilities make Windows service hosting and Spotify command integration more approachable for everyday devs.

- [Running One-Off .NET Tools with dnx: Exploring the .NET 10 Preview](https://andrewlock.net/exploring-dotnet-10-preview-features-5-running-one-off-dotnet-tools-with-dnx/)
- [Introducing .NET Aspire Event Hub Live Explorer](https://www.reddit.com/r/dotnet/comments/1mgm401/introducing_net_aspire_event_hub_live_explorer/)
- [C# Tool to Run Any App as a Windows Service: Managed Alternative to NSSM](https://www.reddit.com/r/dotnet/comments/1mgewsc/just_built_a_tool_that_turns_any_app_into_a/)
- [SpotifyLikeButton: Global Hotkey Utility to Like/Unlike Songs on Spotify](https://www.reddit.com/r/csharp/comments/1mfyig0/spotifylikebutton/)

### Best Practices: Architecture, Validation, and Cross-Platform

Discussions ranged from integrating ASP.NET Core Identity in Clean Architecture, to best practices for FluentValidation centralization. Cross-platform and legacy support using Mono drew caution, while C# inheritance/constructor gotchas resurfaced as valuable reminders for maintainability.

- [How to integrate ASP.NET Core Identity in Clean Architecture (DDD) without breaking domain independence?](https://www.reddit.com/r/dotnet/comments/1meuo7l/how_to_integrate_aspnet_core_identity_in_clean/)
- [Model Validation Best Practices in .NET Using FluentValidation](https://www.reddit.com/r/dotnet/comments/1mg49nf/model_validation_best_practices/)
- [Sanity Check On .NET Framework / Mono / MacOS](https://www.reddit.com/r/dotnet/comments/1mfy7yk/sanity_check_on_net_framework_mono_macos/)
- [C# Inheritance Puzzle](https://www.reddit.com/r/csharp/comments/1mfzryw/c_inheritance_puzzle/)

### UI Modernization and Open Source Community

New C# bindings for Rust’s egui UI, WinUI’s OSS transition, and a wave of community projects (from dashboards to side projects) reflect a robust drive toward desktop UX modernization and continued open source culture in the .NET ecosystem.

- [Egui.NET: unofficial C# bindings for the easy-to-use Rust UI library](https://www.reddit.com/r/csharp/comments/1mgwnvs/eguinet_unofficial_c_bindings_for_the_easytouse/)
- [WinUI OSS Update: Phased Rollout Toward Open Collaboration](https://www.reddit.com/r/dotnet/comments/1mfx9wm/winui_oss_update_phased_rollout_toward_open/)
- [August 2025 Community Project Showcase: C# Side Projects](https://www.reddit.com/r/csharp/comments/1memjhl/come_discuss_your_side_projects_august_2025/)
- [Unlocking Hidden Acrylic/Mica Style UI in Visual Studio 2022](https://www.reddit.com/r/VisualStudio/comments/1mbzcis/visual_studio_2022_has_hidden_acrylicmica_style_ui/)
- [Termix v0.9.0 – Add Rename, Delete, Write File Ops & Fuzzy Search (Preview)](https://www.reddit.com/r/dotnet/comments/1mf1szg/termix_v090_add_rename_delete_write_file_ops/)
- [[Looking for Feedback]: I Made this StateMachine Lib!](https://www.reddit.com/r/csharp/comments/1mfzmdu/looking_for_feedback_i_made_this_statemachine_lib/)

### Dev Events and Knowledge Sharing

Live events and community standups—on Blazor diagnostics, source generators, and SQL—fuel peer-driven learning and rapid open knowledge exchange across the stack.

- [Special Visual Studio Toolbox Live: Microsoft-Led Sessions on .NET, AI, Azure, and Copilot – Aug 5](https://devblogs.microsoft.com/visualstudio/watch-live-visual-studio-toolbox-at-vs-live-redmond-2025/)
- [ASP.NET Community Standup: Building a Better PerfView Diagnostics Tool with Blazor](/coding/videos/aspnet-community-standup-building-a-better-perfview-diagnostics-tool-with-blazor)
- [.NET Data Community Standup: Exploring jOOQ with Lukas Eder and Inspiration for EF](/coding/videos/net-data-community-standup-exploring-jooq-with-lukas-eder-and-inspiration-for-ef)

### Workflow, IDE Troubleshooting, and Productivity

Threads on VS/Windows update pain, IDE choice, effective MVC UX, and code professionalism highlight the practical side of day-to-day dev work, showing knowledge sharing remains key to consistent, maintainable code delivery.

- [Weird Unhandled Exception Errors in Visual Studio After Windows 11 Update](https://www.reddit.com/r/VisualStudio/comments/1mb6app/weird_unhandled_exception_errors_after_windows_11/)
- [Full Stack: Is It Better to Use Visual Studio or VSCode for Back-End and Front-End Development?](https://www.reddit.com/r/dotnet/comments/1mfuefu/full_stack_visual_studio_or_vscode/)
- [How to Show a Spinner on Form Submit Without Disrupting MVC Behavior](https://www.reddit.com/r/dotnet/comments/1mfpq2m/how_do_i_show_a_spinner_btn_on_form_submit/)
- [What does professional code look like?](https://www.reddit.com/r/csharp/comments/1mfsv2g/what_does_professional_code_look_like/)

### Community Engagement and Protocol Contributions

New contributors can directly impact multi-language protocols like MCP, with clear onboarding and open-source guidelines fostering inclusive, scalable progress.

- [How to Build, Test & Deploy MCP Apps with Real Tools and Workflows](/ai/videos/how-to-build-test-and-deploy-mcp-apps-with-real-tools-and-workflows)
- [How to Contribute to MCP: Tools, Documentation, Code & More](/coding/videos/how-to-contribute-to-mcp-tools-documentation-code-and-more)',
    'This week brought strong advances to .NET Aspire, TypeScript, and C# tooling, deeper open-source integration, and community-driven productivity patterns. New releases and workflows point to a modern, flexible Microsoft developer stack supporting both rapid prototyping and scalable, production-grade delivery.',
    1754298000, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2025-08-04', 'TechHub',
    'TechHub', '6ebf3bb413cd4f05d99f2161c388d846', ',.NET Aspire,A2A,Agent Orchestration,AI Agents,C#,Cloud Security,Developer Productivity,Identity Management,MCP,Prompt Engineering,TypeScript,VS Code,AI,GitHub Copilot,ML,Azure,DevOps,Security,Roundups,.NET,',
    false, false, true,
    false, false,
    false, false, 4, NULL
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2025-07-28
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2025-07-28', 'roundups', 'GitHub Copilot Evolves, Multi-Agent AI Expands, and Enterprise Cloud Gets Smarter',
    'This week in coding brought advances in developer ergonomics, automation, and strong community learning—especially for .NET and TypeScript teams.

<!--excerpt_end-->

## Coding

### Source Generator and Protocol Enhancements

.NET 10’s `AddEmbeddedAttributeDefinition()` API eradicates old source generator pain by letting authors embed marker attributes directly—eliminating type conflicts for projects on current SDKs and resulting in cleaner, more maintainable metaprogramming. This follows last week’s .NET preview coverage.

The MCP C# SDK update (protocol 2025-06-18) delivers better OAuth2 support, structured outputs, user information elicitation, and richer metadata—streamlining secure authentication and human-in-the-loop AI workflows.

- [Solving the Source Generator ''Marker Attribute'' Problem in .NET 10: AddEmbeddedAttributeDefinition() Explained](https://andrewlock.net/exploring-dotnet-10-preview-features-4-solving-the-source-generator-marker-attribute-problem-in-dotnet-10/)
- [MCP C# SDK Updated: Protocol 2025-06-18 Brings Elicitation, Structured Output, and Enhanced Security](https://devblogs.microsoft.com/dotnet/mcp-csharp-sdk-2025-06-18-update/)

### Language and Tooling Modernization

TypeScript 5.9 RC introduces ECMAScript `import defer`, Node.js 20 module compatibility, and major speedups, as well as editor goodies like improved tooltips—enabling safer, faster onboarding for teams updating dependencies.

- [Announcing the Release Candidate of TypeScript 5.9: What''s New and Improved](https://devblogs.microsoft.com/typescript/announcing-typescript-5-9-rc/)

### Documentation and Project Standards

PowerShell’s PlatyPS 1.0.0 swaps XML for fast, cross-platform Markdown doc authoring, making up-to-date, source-controlled help a reality for large teams. In .NET Aspire, new name/constant centralization patterns prevent errors and speed up large-scale project refactoring.

- [Announcing Microsoft.PowerShell.PlatyPS 1.0.0: PowerShell Help Authoring Simplified](https://devblogs.microsoft.com/powershell/announcing-platyps-100/)
- [.NET Aspire: Centralizing Project Names and Constants](/coding/videos/net-aspire-centralizing-project-names-and-constants)

### IDE Experience and Community Engagement

A proposed Visual Studio web browser/console extension could cut context-switching and boost web/React/.NET development. Community sessions like ‘Rubber Duck Thursdays’ facilitate hands-on peer learning, reinforcing last week’s focus on collaborative, real-time growth.

- [Developing a Web Browser and Console Log Extension for Visual Studio](https://www.reddit.com/r/VisualStudio/comments/1m5pxx2/possible_new_web_browserconsole_extension/)
- [Rubber Duck Thursdays - Build for the Love of Code](/coding/videos/rubber-duck-thursdays-build-for-the-love-of-code)',
    'This week in coding brought advances in developer ergonomics, automation, and strong community learning—especially for .NET and TypeScript teams.',
    1753693200, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2025-07-28', 'TechHub',
    'TechHub', 'c257f7539204df18c3f934dd17ed6e7f', ',.NET,AI Agents,Cloud Modernization,Enterprise AI,MCP Protocol,Multi Agent Systems,Open Source,Test Automation,TypeScript,VS Code,Workflow Automation,AI,GitHub Copilot,ML,Azure,DevOps,Security,Roundups,',
    false, false, true,
    false, false,
    false, false, 4, NULL
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2025-07-21
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2025-07-21', 'roundups', 'AI Developer Tools and Automation Advances',
    'A host of releases and community solutions empower .NET, VS Code, and Visual Studio users, delivering faster, modernized APIs, automation, cross-language extensibility, and practical tooling.

<!--excerpt_end-->

## Coding

### .NET 10 Features and Extension Member Revolution

.NET 10 Preview and C# 14’s extension members allow seamless type extension—supporting methods, properties, and soon operators—without source modification, yielding discoverable, idiomatic APIs. Preview 6 adds post-quantum cryptography, performance boosts, stateful Blazor, passkey auth, and more, plus faster build and deployment tooling.

- [C# 14 Extension Members in .NET 10 Preview: How to Use Extension Everything](https://andrewlock.net/exploring-dotnet-10-preview-features-3-csharp-14-extensions-members/)
- [.NET 10 Preview 6: New Features Across Runtime, SDK, Libraries, ASP.NET Core, Blazor, and More](https://devblogs.microsoft.com/dotnet/dotnet-10-preview-6/)
- [.NET 10 Preview 6 Unboxed - Blazor State Persistence, Passkey, & What is DNX?](/coding/videos/net-10-preview-6-unboxed-blazor-state-persistence-passkey-and-what-is-dnx)

### AI Tooling, MCP, and .NET Interoperability

New templates and workflows make creating MCP servers in .NET 10 and publishing on NuGet simple, seamlessly blending AI extension and modern extensibility. Python.NET enables embedding Python in C#, expanding .NET’s reach into data science.

- [Building Your First MCP Server with .NET 10 and Publishing to NuGet](https://devblogs.microsoft.com/dotnet/mcp-server-dotnet-nuget-quickstart/)
- [MCP C# SDK: What’s New and Upcoming for .NET Developers](/ai/videos/mcp-c-sdk-whats-new-and-upcoming-for-net-developers)
- [Writing and Running Python in .NET](/coding/videos/writing-and-running-python-in-net)

### Developer Tools, Documentation, and Community Solutions

VS Code’s Markdown Mermaid Viewer solves diagram previewing; a Visual Studio extension adds git worktree support for better branch management. Build-time OpenAPI specs in .NET 9 empower CI/CD and automation. Community standup sessions unpack modern Minimal APIs and database integration, while reusable WPF helpers improve desktop UI. Scott Hanselman spotlights .NET’s broad relevance and stability.

- [VS Code Extension: Preview Mermaid Diagrams in Markdown for Azure DevOps](https://www.reddit.com/r/azuredevops/comments/1m42lmi/vs_code_extension_preview_mermaid_diagrams_in/)
- [Visual Studio Has Most Git Features I Need—Except Worktree, So I Built an Extension for It](https://www.reddit.com/r/VisualStudio/comments/1m1l1lc/visual_studio_has_most_git_features_i_need_except/)
- [Build-Time OpenAPI Documentation in .NET 9: A OneDevQuestion with Mike Kistler](/coding/videos/build-time-openapi-documentation-in-net-9-a-onedevquestion-with-mike-kistler)
- [.NET Data Community Standup: Couchbase EF Core Provider Discussion](/coding/videos/net-data-community-standup-couchbase-ef-core-provider-discussion)
- [ASP.NET Community Standup - Why Aren''t You Using Minimal APIs?](/coding/videos/aspnet-community-standup-why-arent-you-using-minimal-apis)
- [Centering a WPF TreeViewItem in the TreeView ScrollViewer](https://weblog.west-wind.com/posts/2025/Jul/15/Centering-a-WPF-TreeViewItem-in-the-TreeView-ScrollViewer)
- [Is .NET Legacy Tech? Scott Hanselman Explores the Modern .NET Platform](/coding/videos/is-net-legacy-tech-scott-hanselman-explores-the-modern-net-platform)',
    'A host of releases and community solutions empower .NET, VS Code, and Visual Studio users, delivering faster, modernized APIs, automation, cross-language extensibility, and practical tooling.',
    1753088400, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2025-07-21', 'TechHub',
    'TechHub', '25e36a020b1c74460a195f7c5c8ef3e7', ',.NET 10,Agentic AI,Automation,Coding Agents,Compliance,MCP,VS,VS Code,AI,GitHub Copilot,ML,Azure,DevOps,Security,Roundups,.NET,',
    false, false, true,
    false, false,
    false, false, 4, NULL
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2025-07-14
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2025-07-14', 'roundups', 'From Copilot Agents to AI-Driven Security: Key Platform and Productivity Advances',
    'Tooling and best practices saw key advances for TypeScript, .NET, and MVVM, all supporting more productive, maintainable workflows.

<!--excerpt_end-->

## Coding

### TypeScript and .NET: Configuration, Tooling, and Performance

TypeScript 5.9 Beta streamlines `tsc --init` for rapid, default-rich starts. New features—ECMAScript ''import defer'', Node.js v20 stable modules, and enhanced DOM types—ease full-stack development. Editor refinements aid navigation and debugging, while type system optimizations yield quicker builds. .NET’s July releases fix vulnerabilities and boost reliability. Andrew Lock’s dive into .NET 10’s single-file run experience (`dotnet run app.cs`) modernizes scripting, supporting dependency management and build properties within C# for fast prototyping.

This extends last week’s coverage on .NET scripting innovations and dev productivity initiatives.

- [Announcing TypeScript 5.9 Beta: New Features, Improvements, and Optimizations](https://devblogs.microsoft.com/typescript/announcing-typescript-5-9-beta/)
- [Behind the Scenes of dotnet run app.cs: Deep Dive into .NET 10 Single-File Run Experience](https://andrewlock.net/exploring-dotnet-10-preview-features-2-behind-the-scenes-of-dotnet-run-app.cs/)
- [.NET and .NET Framework July 2025 Servicing Releases Updates](https://devblogs.microsoft.com/dotnet/dotnet-and-dotnet-framework-july-2025-servicing-updates/)

### Secure Dependency Management and MVVM Decoupling

A .NET-focused guide clarifies upgrade strategies: to remediate vulnerabilities, update NuGet packages at the project level, not just system runtimes. This is vital for accurate scanning and secure CI/CD. For WPF/MVVM, new advice details decoupling Views from ViewModels using ViewModelLocator patterns and modern DI, making UI logic more modular and testable.

- [Upgrading NuGet Packages vs. .NET Runtime: Addressing SCA Vulnerabilities in Microsoft.AspNetCore.*](https://techcommunity.microsoft.com/t5/net-runtime/do-i-need-to-upgrade-microsoft-aspnetcore-nuget-packages-after/m-p/4431436#M752)
- [Decoupling Views and ViewModels in CommunityToolkit.Mvvm](https://techcommunity.microsoft.com/t5/app-development/how-to-decouple-views-from-view-models-using-communitytoolkit/m-p/4432591#M1261)',
    'Tooling and best practices saw key advances for TypeScript, .NET, and MVVM, all supporting more productive, maintainable workflows.',
    1752483600, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2025-07-14', 'TechHub',
    'TechHub', '842cae485c44d65eb85996069f491f4c', ',.NET,AI Agents,AI Ethics,Automation,CI/CD,Cloud Migration,Microsoft,MCP,Prompt Engineering,Semantic Kernel,TypeScript,Zero Trust,AI,GitHub Copilot,ML,Azure,DevOps,Security,Roundups,',
    false, false, true,
    false, false,
    false, false, 4, NULL
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();
-- weekly-dotnet-roundup-2025-07-07
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', 'GitHub Copilot Agent Preview, Azure AI Foundry GA, and Smarter .NET Workflows',
    '.NET sees continued push toward frictionless, cross-platform development—condensing mobile/web convergence, AI enhancements, and rapid scripting into actionable productivity boosters.

<!--excerpt_end-->

## Coding

### Single-File Apps and Modern Scripting in .NET 10

.NET 10 Preview enables running full C# apps from a single `.cs` file using `dotnet run app.cs`. Andrew Lock’s analysis shows how to include in-file NuGet/SDK directives, leverage shebang for Unix-like environments, and notes forthcoming improvements such as multi-file support. This drastically lowers entry barriers for prototyping, sharing, and experimenting without the need for complex project setups.

- [Exploring Single-File .NET Apps with dotnet run app.cs in .NET 10 Preview](https://andrewlock.net/exploring-dotnet-10-preview-features-1-exploring-the-dotnet-run-app.cs/)

### Blazor, MAUI, and AI-Enhanced Mobile Experiences

Through the .NET MAUI standup, Beth Massi and David Ortinau demonstrate how Blazor web apps can be rapidly adapted for native mobile platforms with full device feature access, while Azure AI Foundry tooling streamlines embedding AI. Community-driven resources and best practices further empower web/mobile/AI hybrid programming.

- [.NET MAUI Community Standup - Blazor for Mobile with AI? Here''s how.](/ai/videos/net-maui-community-standup-blazor-for-mobile-with-ai-heres-how)',
    '.NET sees continued push toward frictionless, cross-platform development—condensing mobile/web convergence, AI enhancements, and rapid scripting into actionable productivity boosters.',
    1751878800, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2025-07-07', 'TechHub',
    'TechHub', '3e6db5534a59701b6de4052a88808715', ',.NET 10,AI Accessibility,AI Agents,Azure AI Foundry,Azure Dev CLI,Azure Files,Blazor,Cloud Security,Copilot Agent,Copilot Vision,Cosmos DB,MAUI,MCP,Ollama,PostgreSQL,Semantic Kernel,Terraform,Vault,AI,GitHub Copilot,ML,Azure,DevOps,Security,Roundups,.NET,',
    false, false, true,
    false, false,
    false, false, 4, NULL
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title              = EXCLUDED.title,
    content            = EXCLUDED.content,
    excerpt            = EXCLUDED.excerpt,
    date_epoch         = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url       = EXCLUDED.external_url,
    author             = EXCLUDED.author,
    feed_name          = EXCLUDED.feed_name,
    content_hash       = EXCLUDED.content_hash,
    tags_csv           = EXCLUDED.tags_csv,
    is_ai              = EXCLUDED.is_ai,
    is_azure           = EXCLUDED.is_azure,
    is_dotnet          = EXCLUDED.is_dotnet,
    is_devops          = EXCLUDED.is_devops,
    is_github_copilot  = EXCLUDED.is_github_copilot,
    is_ml              = EXCLUDED.is_ml,
    is_security        = EXCLUDED.is_security,
    sections_bitmask   = EXCLUDED.sections_bitmask,
    ai_metadata        = EXCLUDED.ai_metadata,
    updated_at         = NOW();


-- ── content_tags_expanded ───────────────────────────────────────────────

INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', '.net', '.NET', false,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', '.net 10', '.NET 10', true,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', '10', '10', false,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', 'accessibility', 'Accessibility', false,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', 'agent', 'Agent', false,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', 'agents', 'Agents', false,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', 'ai', 'AI', false,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', 'ai accessibility', 'AI Accessibility', true,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', 'ai agents', 'AI Agents', true,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', 'azure', 'Azure', false,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', 'azure ai foundry', 'Azure AI Foundry', true,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', 'azure dev cli', 'Azure Dev CLI', true,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', 'azure files', 'Azure Files', true,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', 'blazor', 'Blazor', true,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', 'cli', 'CLI', false,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', 'cloud', 'Cloud', false,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', 'cloud security', 'Cloud Security', true,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', 'copilot', 'Copilot', false,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', 'copilot agent', 'Copilot Agent', true,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', 'copilot vision', 'Copilot Vision', true,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', 'cosmos', 'Cosmos', false,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', 'cosmos db', 'Cosmos DB', true,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', 'db', 'DB', false,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', 'dev', 'Dev', false,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', 'devops', 'DevOps', true,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', 'files', 'Files', false,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', 'foundry', 'Foundry', false,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', 'github', 'GitHub', false,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', 'kernel', 'Kernel', false,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', 'maui', 'MAUI', true,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', 'mcp', 'MCP', true,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', 'ml', 'ML', true,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', 'ollama', 'Ollama', true,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', 'postgresql', 'PostgreSQL', true,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', 'roundups', 'Roundups', true,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', 'security', 'Security', false,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', 'semantic', 'Semantic', false,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', 'semantic kernel', 'Semantic Kernel', true,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', 'terraform', 'Terraform', true,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', 'vault', 'Vault', true,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-07', 'roundups', 'vision', 'Vision', false,
    1751878800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-14', 'roundups', '(mcp)', '(mcp)', false,
    1752483600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-14', 'roundups', '.net', '.NET', true,
    1752483600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-14', 'roundups', 'agents', 'Agents', false,
    1752483600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-14', 'roundups', 'ai', 'AI', false,
    1752483600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-14', 'roundups', 'ai agents', 'AI Agents', true,
    1752483600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-14', 'roundups', 'ai ethics', 'AI Ethics', true,
    1752483600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-14', 'roundups', 'automation', 'Automation', true,
    1752483600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-14', 'roundups', 'azure', 'Azure', true,
    1752483600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-14', 'roundups', 'ci/cd', 'CI/CD', true,
    1752483600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-14', 'roundups', 'cloud', 'Cloud', false,
    1752483600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-14', 'roundups', 'cloud migration', 'Cloud Migration', true,
    1752483600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-14', 'roundups', 'context', 'Context', false,
    1752483600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-14', 'roundups', 'copilot', 'Copilot', false,
    1752483600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-14', 'roundups', 'devops', 'DevOps', true,
    1752483600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-14', 'roundups', 'engineering', 'Engineering', false,
    1752483600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-14', 'roundups', 'ethics', 'Ethics', false,
    1752483600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-14', 'roundups', 'github', 'GitHub', false,
    1752483600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-14', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1752483600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-14', 'roundups', 'kernel', 'Kernel', false,
    1752483600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-14', 'roundups', 'microsoft', 'Microsoft', true,
    1752483600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-14', 'roundups', 'migration', 'Migration', false,
    1752483600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-14', 'roundups', 'ml', 'ML', true,
    1752483600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-14', 'roundups', 'model', 'Model', false,
    1752483600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-14', 'roundups', 'model context protocol (mcp)', 'Model Context Protocol (mcp)', true,
    1752483600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-14', 'roundups', 'prompt', 'Prompt', false,
    1752483600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-14', 'roundups', 'prompt engineering', 'Prompt Engineering', true,
    1752483600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-14', 'roundups', 'protocol', 'Protocol', false,
    1752483600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-14', 'roundups', 'roundups', 'Roundups', true,
    1752483600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-14', 'roundups', 'security', 'Security', true,
    1752483600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-14', 'roundups', 'semantic', 'Semantic', false,
    1752483600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-14', 'roundups', 'semantic kernel', 'Semantic Kernel', true,
    1752483600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-14', 'roundups', 'trust', 'Trust', false,
    1752483600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-14', 'roundups', 'typescript', 'TypeScript', true,
    1752483600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-14', 'roundups', 'zero', 'Zero', false,
    1752483600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-14', 'roundups', 'zero trust', 'Zero Trust', true,
    1752483600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-21', 'roundups', '.net', '.NET', false,
    1753088400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-21', 'roundups', '.net 10', '.NET 10', true,
    1753088400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-21', 'roundups', '10', '10', false,
    1753088400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-21', 'roundups', 'agentic', 'Agentic', false,
    1753088400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-21', 'roundups', 'agentic ai', 'Agentic AI', true,
    1753088400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-21', 'roundups', 'agents', 'Agents', false,
    1753088400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-21', 'roundups', 'ai', 'AI', false,
    1753088400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-21', 'roundups', 'automation', 'Automation', true,
    1753088400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-21', 'roundups', 'azure', 'Azure', true,
    1753088400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-21', 'roundups', 'code', 'Code', false,
    1753088400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-21', 'roundups', 'coding', 'Coding', false,
    1753088400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-21', 'roundups', 'coding agents', 'Coding Agents', true,
    1753088400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-21', 'roundups', 'compliance', 'Compliance', true,
    1753088400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-21', 'roundups', 'copilot', 'Copilot', false,
    1753088400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-21', 'roundups', 'devops', 'DevOps', true,
    1753088400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-21', 'roundups', 'github', 'GitHub', false,
    1753088400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-21', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1753088400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-21', 'roundups', 'mcp', 'MCP', true,
    1753088400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-21', 'roundups', 'ml', 'ML', true,
    1753088400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-21', 'roundups', 'roundups', 'Roundups', true,
    1753088400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-21', 'roundups', 'security', 'Security', true,
    1753088400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-21', 'roundups', 'vs', 'VS', true,
    1753088400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-21', 'roundups', 'vs code', 'VS Code', true,
    1753088400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-28', 'roundups', '.net', '.NET', true,
    1753693200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-28', 'roundups', 'agent', 'Agent', false,
    1753693200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-28', 'roundups', 'agents', 'Agents', false,
    1753693200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-28', 'roundups', 'ai', 'AI', false,
    1753693200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-28', 'roundups', 'ai agents', 'AI Agents', true,
    1753693200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-28', 'roundups', 'automation', 'Automation', false,
    1753693200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-28', 'roundups', 'azure', 'Azure', true,
    1753693200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-28', 'roundups', 'cloud', 'Cloud', false,
    1753693200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-28', 'roundups', 'cloud modernization', 'Cloud Modernization', true,
    1753693200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-28', 'roundups', 'code', 'Code', false,
    1753693200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-28', 'roundups', 'copilot', 'Copilot', false,
    1753693200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-28', 'roundups', 'devops', 'DevOps', true,
    1753693200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-28', 'roundups', 'enterprise', 'Enterprise', false,
    1753693200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-28', 'roundups', 'enterprise ai', 'Enterprise AI', true,
    1753693200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-28', 'roundups', 'github', 'GitHub', false,
    1753693200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-28', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1753693200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-28', 'roundups', 'mcp', 'MCP', false,
    1753693200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-28', 'roundups', 'mcp protocol', 'MCP Protocol', true,
    1753693200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-28', 'roundups', 'ml', 'ML', true,
    1753693200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-28', 'roundups', 'modernization', 'Modernization', false,
    1753693200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-28', 'roundups', 'multi', 'Multi', false,
    1753693200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-28', 'roundups', 'multi agent systems', 'Multi Agent Systems', true,
    1753693200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-28', 'roundups', 'open', 'Open', false,
    1753693200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-28', 'roundups', 'open source', 'Open Source', true,
    1753693200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-28', 'roundups', 'protocol', 'Protocol', false,
    1753693200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-28', 'roundups', 'roundups', 'Roundups', true,
    1753693200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-28', 'roundups', 'security', 'Security', true,
    1753693200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-28', 'roundups', 'source', 'Source', false,
    1753693200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-28', 'roundups', 'systems', 'Systems', false,
    1753693200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-28', 'roundups', 'test', 'Test', false,
    1753693200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-28', 'roundups', 'test automation', 'Test Automation', true,
    1753693200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-28', 'roundups', 'typescript', 'TypeScript', true,
    1753693200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-28', 'roundups', 'vs', 'VS', false,
    1753693200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-28', 'roundups', 'vs code', 'VS Code', true,
    1753693200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-28', 'roundups', 'workflow', 'Workflow', false,
    1753693200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-07-28', 'roundups', 'workflow automation', 'Workflow Automation', true,
    1753693200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-04', 'roundups', '.net', '.NET', false,
    1754298000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-04', 'roundups', '.net aspire', '.NET Aspire', true,
    1754298000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-04', 'roundups', 'a2a', 'A2A', true,
    1754298000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-04', 'roundups', 'agent', 'Agent', false,
    1754298000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-04', 'roundups', 'agent orchestration', 'Agent Orchestration', true,
    1754298000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-04', 'roundups', 'agents', 'Agents', false,
    1754298000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-04', 'roundups', 'ai', 'AI', false,
    1754298000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-04', 'roundups', 'ai agents', 'AI Agents', true,
    1754298000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-04', 'roundups', 'aspire', 'Aspire', false,
    1754298000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-04', 'roundups', 'azure', 'Azure', true,
    1754298000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-04', 'roundups', 'c#', 'C#', true,
    1754298000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-04', 'roundups', 'cloud', 'Cloud', false,
    1754298000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-04', 'roundups', 'cloud security', 'Cloud Security', true,
    1754298000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-04', 'roundups', 'code', 'Code', false,
    1754298000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-04', 'roundups', 'copilot', 'Copilot', false,
    1754298000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-04', 'roundups', 'developer', 'Developer', false,
    1754298000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-04', 'roundups', 'developer productivity', 'Developer Productivity', true,
    1754298000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-04', 'roundups', 'devops', 'DevOps', true,
    1754298000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-04', 'roundups', 'engineering', 'Engineering', false,
    1754298000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-04', 'roundups', 'github', 'GitHub', false,
    1754298000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-04', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1754298000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-04', 'roundups', 'identity', 'Identity', false,
    1754298000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-04', 'roundups', 'identity management', 'Identity Management', true,
    1754298000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-04', 'roundups', 'management', 'Management', false,
    1754298000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-04', 'roundups', 'mcp', 'MCP', true,
    1754298000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-04', 'roundups', 'ml', 'ML', true,
    1754298000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-04', 'roundups', 'orchestration', 'Orchestration', false,
    1754298000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-04', 'roundups', 'productivity', 'Productivity', false,
    1754298000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-04', 'roundups', 'prompt', 'Prompt', false,
    1754298000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-04', 'roundups', 'prompt engineering', 'Prompt Engineering', true,
    1754298000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-04', 'roundups', 'roundups', 'Roundups', true,
    1754298000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-04', 'roundups', 'security', 'Security', false,
    1754298000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-04', 'roundups', 'typescript', 'TypeScript', true,
    1754298000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-04', 'roundups', 'vs', 'VS', false,
    1754298000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-04', 'roundups', 'vs code', 'VS Code', true,
    1754298000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-11', 'roundups', '.net', '.NET', true,
    1754902800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-11', 'roundups', '4.1', '4.1', false,
    1754902800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-11', 'roundups', '5', '5', false,
    1754902800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-11', 'roundups', 'agent', 'Agent', false,
    1754902800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-11', 'roundups', 'agents', 'Agents', false,
    1754902800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-11', 'roundups', 'ai', 'AI', false,
    1754902800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-11', 'roundups', 'ai agents', 'AI Agents', true,
    1754902800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-11', 'roundups', 'authentication', 'Authentication', false,
    1754902800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-11', 'roundups', 'azure', 'Azure', true,
    1754902800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-11', 'roundups', 'ci/cd', 'CI/CD', true,
    1754902800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-11', 'roundups', 'claude', 'Claude', false,
    1754902800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-11', 'roundups', 'claude opus 4.1', 'Claude Opus 4.1', true,
    1754902800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-11', 'roundups', 'cloud', 'Cloud', false,
    1754902800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-11', 'roundups', 'cloud identity', 'Cloud Identity', true,
    1754902800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-11', 'roundups', 'code', 'Code', false,
    1754902800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-11', 'roundups', 'copilot', 'Copilot', false,
    1754902800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-11', 'roundups', 'devops', 'DevOps', true,
    1754902800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-11', 'roundups', 'enterprise', 'Enterprise', false,
    1754902800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-11', 'roundups', 'enterprise ai', 'Enterprise AI', true,
    1754902800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-11', 'roundups', 'github', 'GitHub', false,
    1754902800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-11', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1754902800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-11', 'roundups', 'gpt', 'GPT', false,
    1754902800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-11', 'roundups', 'gpt 5', 'GPT 5', true,
    1754902800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-11', 'roundups', 'identity', 'Identity', false,
    1754902800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-11', 'roundups', 'mcp', 'MCP', true,
    1754902800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-11', 'roundups', 'ml', 'ML', true,
    1754902800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-11', 'roundups', 'multi', 'Multi', false,
    1754902800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-11', 'roundups', 'multi agent systems', 'Multi Agent Systems', true,
    1754902800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-11', 'roundups', 'opus', 'Opus', false,
    1754902800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-11', 'roundups', 'passwordless', 'Passwordless', false,
    1754902800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-11', 'roundups', 'passwordless authentication', 'Passwordless Authentication', true,
    1754902800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-11', 'roundups', 'roundups', 'Roundups', true,
    1754902800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-11', 'roundups', 'security', 'Security', true,
    1754902800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-11', 'roundups', 'systems', 'Systems', false,
    1754902800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-11', 'roundups', 'vs', 'VS', false,
    1754902800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-11', 'roundups', 'vs code', 'VS Code', true,
    1754902800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', '.net', '.NET', true,
    1755507600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', '5', '5', false,
    1755507600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', 'agentic', 'Agentic', false,
    1755507600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', 'agentic ai', 'Agentic AI', true,
    1755507600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', 'ai', 'AI', false,
    1755507600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', 'ai tools', 'AI Tools', true,
    1755507600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', 'analytics', 'Analytics', false,
    1755507600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', 'application', 'Application', false,
    1755507600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', 'application security', 'Application Security', true,
    1755507600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', 'automation', 'Automation', false,
    1755507600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', 'azure', 'Azure', true,
    1755507600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', 'cloud', 'Cloud', false,
    1755507600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', 'cloud security', 'Cloud Security', true,
    1755507600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', 'code', 'Code', false,
    1755507600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', 'copilot', 'Copilot', false,
    1755507600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', 'copilot studio', 'Copilot Studio', true,
    1755507600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', 'data', 'Data', false,
    1755507600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', 'data analytics', 'Data Analytics', true,
    1755507600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', 'devops', 'DevOps', true,
    1755507600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', 'enterprise', 'Enterprise', false,
    1755507600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', 'enterprise automation', 'Enterprise Automation', true,
    1755507600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', 'fabric', 'Fabric', false,
    1755507600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', 'github', 'GitHub', false,
    1755507600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1755507600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', 'gpt', 'GPT', false,
    1755507600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', 'gpt 5', 'GPT 5', true,
    1755507600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', 'llms', 'LLMs', true,
    1755507600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', 'mcp', 'MCP', true,
    1755507600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', 'microsoft', 'Microsoft', false,
    1755507600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', 'microsoft fabric', 'Microsoft Fabric', true,
    1755507600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', 'ml', 'ML', true,
    1755507600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', 'open', 'Open', false,
    1755507600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', 'open source', 'Open Source', true,
    1755507600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', 'roundups', 'Roundups', true,
    1755507600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', 'security', 'Security', false,
    1755507600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', 'source', 'Source', false,
    1755507600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', 'studio', 'Studio', false,
    1755507600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', 'tools', 'Tools', false,
    1755507600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', 'vs', 'VS', false,
    1755507600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-18', 'roundups', 'vs code', 'VS Code', true,
    1755507600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-25', 'roundups', '.net', '.NET', false,
    1756112400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-25', 'roundups', '.net modernization', '.NET Modernization', true,
    1756112400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-25', 'roundups', 'agents', 'Agents', false,
    1756112400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-25', 'roundups', 'ai', 'AI', false,
    1756112400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-25', 'roundups', 'ai agents', 'AI Agents', true,
    1756112400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-25', 'roundups', 'automation', 'Automation', false,
    1756112400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-25', 'roundups', 'azure', 'Azure', false,
    1756112400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-25', 'roundups', 'azure updates', 'Azure Updates', true,
    1756112400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-25', 'roundups', 'ci/cd', 'CI/CD', false,
    1756112400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-25', 'roundups', 'ci/cd pipelines', 'CI/CD Pipelines', true,
    1756112400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-25', 'roundups', 'cloud', 'Cloud', false,
    1756112400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-25', 'roundups', 'cloud infrastructure', 'Cloud Infrastructure', true,
    1756112400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-25', 'roundups', 'copilot', 'Copilot', false,
    1756112400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-25', 'roundups', 'developer', 'Developer', false,
    1756112400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-25', 'roundups', 'developer productivity', 'Developer Productivity', true,
    1756112400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-25', 'roundups', 'devops', 'DevOps', false,
    1756112400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-25', 'roundups', 'devops automation', 'DevOps Automation', true,
    1756112400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-25', 'roundups', 'enterprise', 'Enterprise', false,
    1756112400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-25', 'roundups', 'enterprise security', 'Enterprise Security', true,
    1756112400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-25', 'roundups', 'git', 'Git', true,
    1756112400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-25', 'roundups', 'github', 'GitHub', false,
    1756112400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-25', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1756112400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-25', 'roundups', 'infrastructure', 'Infrastructure', false,
    1756112400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-25', 'roundups', 'mcp', 'MCP', true,
    1756112400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-25', 'roundups', 'ml', 'ML', false,
    1756112400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-25', 'roundups', 'ml optimization', 'ML Optimization', true,
    1756112400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-25', 'roundups', 'modernization', 'Modernization', false,
    1756112400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-25', 'roundups', 'optimization', 'Optimization', false,
    1756112400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-25', 'roundups', 'pipelines', 'Pipelines', false,
    1756112400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-25', 'roundups', 'productivity', 'Productivity', false,
    1756112400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-25', 'roundups', 'quantum', 'Quantum', false,
    1756112400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-25', 'roundups', 'quantum safe security', 'Quantum Safe Security', true,
    1756112400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-25', 'roundups', 'roundups', 'Roundups', true,
    1756112400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-25', 'roundups', 'safe', 'Safe', false,
    1756112400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-25', 'roundups', 'security', 'Security', false,
    1756112400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-25', 'roundups', 'updates', 'Updates', false,
    1756112400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-25', 'roundups', 'vs', 'VS', true,
    1756112400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-25', 'roundups', 'workflow', 'Workflow', false,
    1756112400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-08-25', 'roundups', 'workflow automation', 'Workflow Automation', true,
    1756112400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-01', 'roundups', '.net', '.NET', true,
    1756717200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-01', 'roundups', 'agents', 'Agents', false,
    1756717200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-01', 'roundups', 'ai', 'AI', false,
    1756717200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-01', 'roundups', 'ai agents', 'AI Agents', true,
    1756717200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-01', 'roundups', 'automation', 'Automation', false,
    1756717200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-01', 'roundups', 'azure', 'Azure', true,
    1756717200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-01', 'roundups', 'benchmarking', 'Benchmarking', false,
    1756717200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-01', 'roundups', 'chain', 'Chain', false,
    1756717200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-01', 'roundups', 'cloud', 'Cloud', false,
    1756717200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-01', 'roundups', 'cloud infrastructure', 'Cloud Infrastructure', true,
    1756717200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-01', 'roundups', 'copilot', 'Copilot', false,
    1756717200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-01', 'roundups', 'data', 'Data', false,
    1756717200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-01', 'roundups', 'data engineering', 'Data Engineering', true,
    1756717200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-01', 'roundups', 'developer', 'Developer', false,
    1756717200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-01', 'roundups', 'developer productivity', 'Developer Productivity', true,
    1756717200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-01', 'roundups', 'devops', 'DevOps', true,
    1756717200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-01', 'roundups', 'engineering', 'Engineering', false,
    1756717200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-01', 'roundups', 'github', 'GitHub', false,
    1756717200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-01', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1756717200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-01', 'roundups', 'infrastructure', 'Infrastructure', false,
    1756717200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-01', 'roundups', 'kernel', 'Kernel', false,
    1756717200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-01', 'roundups', 'kubernetes', 'Kubernetes', true,
    1756717200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-01', 'roundups', 'llm', 'LLM', false,
    1756717200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-01', 'roundups', 'llm benchmarking', 'LLM Benchmarking', true,
    1756717200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-01', 'roundups', 'ml', 'ML', true,
    1756717200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-01', 'roundups', 'open', 'Open', false,
    1756717200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-01', 'roundups', 'open source', 'Open Source', true,
    1756717200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-01', 'roundups', 'platform', 'Platform', false,
    1756717200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-01', 'roundups', 'platform automation', 'Platform Automation', true,
    1756717200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-01', 'roundups', 'productivity', 'Productivity', false,
    1756717200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-01', 'roundups', 'roundups', 'Roundups', true,
    1756717200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-01', 'roundups', 'security', 'Security', false,
    1756717200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-01', 'roundups', 'semantic', 'Semantic', false,
    1756717200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-01', 'roundups', 'semantic kernel', 'Semantic Kernel', true,
    1756717200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-01', 'roundups', 'source', 'Source', false,
    1756717200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-01', 'roundups', 'supply', 'Supply', false,
    1756717200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-01', 'roundups', 'supply chain security', 'Supply Chain Security', true,
    1756717200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', '.net', '.NET', true,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'agents', 'Agents', false,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'ai', 'AI', false,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'ai agents', 'AI Agents', true,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'analytics', 'Analytics', false,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'automation', 'Automation', false,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'azure', 'Azure', false,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'azure ai', 'Azure AI', true,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'blazor', 'Blazor', true,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'ci/cd', 'CI/CD', true,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'cloud', 'Cloud', false,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'cloud infrastructure', 'Cloud Infrastructure', true,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'code', 'Code', false,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'code review', 'Code Review', true,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'compliance', 'Compliance', true,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'copilot', 'Copilot', false,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'data', 'Data', false,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'data analytics', 'Data Analytics', true,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'developer', 'Developer', false,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'developer tooling', 'Developer Tooling', true,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'devops', 'DevOps', true,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'enterprise', 'Enterprise', false,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'enterprise security', 'Enterprise Security', true,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'fabric', 'Fabric', false,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'github', 'GitHub', false,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'identity', 'Identity', false,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'identity management', 'Identity Management', true,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'infrastructure', 'Infrastructure', false,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'kubernetes', 'Kubernetes', true,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'management', 'Management', false,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'mcp', 'MCP', true,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'microsoft', 'Microsoft', false,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'microsoft fabric', 'Microsoft Fabric', true,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'ml', 'ML', true,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'openai', 'OpenAI', true,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'review', 'Review', false,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'roundups', 'Roundups', true,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'security', 'Security', false,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'tooling', 'Tooling', false,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'vs', 'VS', false,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'vs code', 'VS Code', true,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'workflow', 'Workflow', false,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-08', 'roundups', 'workflow automation', 'Workflow Automation', true,
    1757322000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-15', 'roundups', '.net', '.NET', true,
    1757926800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-15', 'roundups', 'ai', 'AI', false,
    1757926800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-15', 'roundups', 'ai coding', 'AI Coding', true,
    1757926800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-15', 'roundups', 'ai infrastructure', 'AI Infrastructure', true,
    1757926800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-15', 'roundups', 'apps', 'Apps', false,
    1757926800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-15', 'roundups', 'automation', 'Automation', false,
    1757926800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-15', 'roundups', 'azure', 'Azure', true,
    1757926800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-15', 'roundups', 'ci/cd', 'CI/CD', true,
    1757926800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-15', 'roundups', 'cloud', 'Cloud', false,
    1757926800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-15', 'roundups', 'cloud computing', 'Cloud Computing', true,
    1757926800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-15', 'roundups', 'code', 'Code', false,
    1757926800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-15', 'roundups', 'coding', 'Coding', false,
    1757926800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-15', 'roundups', 'computing', 'Computing', false,
    1757926800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-15', 'roundups', 'copilot', 'Copilot', false,
    1757926800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-15', 'roundups', 'data', 'Data', false,
    1757926800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-15', 'roundups', 'data management', 'Data Management', true,
    1757926800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-15', 'roundups', 'devops', 'DevOps', true,
    1757926800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-15', 'roundups', 'enterprise', 'Enterprise', false,
    1757926800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-15', 'roundups', 'enterprise apps', 'Enterprise Apps', true,
    1757926800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-15', 'roundups', 'github', 'GitHub', false,
    1757926800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-15', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1757926800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-15', 'roundups', 'infrastructure', 'Infrastructure', false,
    1757926800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-15', 'roundups', 'low', 'Low', false,
    1757926800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-15', 'roundups', 'low code automation', 'Low Code Automation', true,
    1757926800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-15', 'roundups', 'management', 'Management', false,
    1757926800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-15', 'roundups', 'ml', 'ML', true,
    1757926800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-15', 'roundups', 'observability', 'Observability', true,
    1757926800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-15', 'roundups', 'open', 'Open', false,
    1757926800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-15', 'roundups', 'open source', 'Open Source', true,
    1757926800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-15', 'roundups', 'roundups', 'Roundups', true,
    1757926800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-15', 'roundups', 'security', 'Security', true,
    1757926800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-15', 'roundups', 'source', 'Source', false,
    1757926800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-15', 'roundups', 'vs', 'VS', false,
    1757926800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-15', 'roundups', 'vs code', 'VS Code', true,
    1757926800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-22', 'roundups', '(model', '(model', false,
    1758531600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-22', 'roundups', '.net', '.NET', true,
    1758531600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-22', 'roundups', 'agents', 'Agents', false,
    1758531600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-22', 'roundups', 'ai', 'AI', false,
    1758531600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-22', 'roundups', 'ai agents', 'AI Agents', true,
    1758531600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-22', 'roundups', 'application', 'Application', false,
    1758531600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-22', 'roundups', 'application modernization', 'Application Modernization', true,
    1758531600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-22', 'roundups', 'automation', 'Automation', true,
    1758531600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-22', 'roundups', 'azure', 'Azure', true,
    1758531600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-22', 'roundups', 'cloud', 'Cloud', false,
    1758531600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-22', 'roundups', 'cloud migration', 'Cloud Migration', true,
    1758531600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-22', 'roundups', 'computing', 'Computing', false,
    1758531600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-22', 'roundups', 'context', 'Context', false,
    1758531600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-22', 'roundups', 'copilot', 'Copilot', false,
    1758531600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-22', 'roundups', 'developer', 'Developer', false,
    1758531600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-22', 'roundups', 'developer tools', 'Developer Tools', true,
    1758531600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-22', 'roundups', 'devops', 'DevOps', true,
    1758531600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-22', 'roundups', 'fabric', 'Fabric', false,
    1758531600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-22', 'roundups', 'github', 'GitHub', false,
    1758531600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-22', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1758531600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-22', 'roundups', 'kubernetes', 'Kubernetes', true,
    1758531600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-22', 'roundups', 'language', 'Language', false,
    1758531600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-22', 'roundups', 'large', 'Large', false,
    1758531600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-22', 'roundups', 'large language models', 'Large Language Models', true,
    1758531600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-22', 'roundups', 'mcp', 'MCP', false,
    1758531600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-22', 'roundups', 'mcp (model context protocol)', 'MCP (model Context Protocol)', true,
    1758531600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-22', 'roundups', 'microsoft', 'Microsoft', false,
    1758531600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-22', 'roundups', 'microsoft fabric', 'Microsoft Fabric', true,
    1758531600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-22', 'roundups', 'migration', 'Migration', false,
    1758531600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-22', 'roundups', 'ml', 'ML', true,
    1758531600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-22', 'roundups', 'models', 'Models', false,
    1758531600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-22', 'roundups', 'modernization', 'Modernization', false,
    1758531600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-22', 'roundups', 'protocol)', 'Protocol)', false,
    1758531600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-22', 'roundups', 'quantum', 'Quantum', false,
    1758531600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-22', 'roundups', 'quantum computing', 'Quantum Computing', true,
    1758531600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-22', 'roundups', 'roundups', 'Roundups', true,
    1758531600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-22', 'roundups', 'security', 'Security', true,
    1758531600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-22', 'roundups', 'tools', 'Tools', false,
    1758531600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-29', 'roundups', '.net', '.NET', true,
    1759136400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-29', 'roundups', 'agentic', 'Agentic', false,
    1759136400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-29', 'roundups', 'agentic automation', 'Agentic Automation', true,
    1759136400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-29', 'roundups', 'agents', 'Agents', false,
    1759136400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-29', 'roundups', 'ai', 'AI', false,
    1759136400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-29', 'roundups', 'ai agents', 'AI Agents', true,
    1759136400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-29', 'roundups', 'automation', 'Automation', false,
    1759136400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-29', 'roundups', 'azure', 'Azure', false,
    1759136400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-29', 'roundups', 'azure ai', 'Azure AI', true,
    1759136400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-29', 'roundups', 'chain', 'Chain', false,
    1759136400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-29', 'roundups', 'claude', 'Claude', false,
    1759136400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-29', 'roundups', 'claude opus', 'Claude Opus', true,
    1759136400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-29', 'roundups', 'cloud', 'Cloud', false,
    1759136400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-29', 'roundups', 'cloud modernization', 'Cloud Modernization', true,
    1759136400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-29', 'roundups', 'containerization', 'Containerization', true,
    1759136400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-29', 'roundups', 'copilot', 'Copilot', false,
    1759136400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-29', 'roundups', 'devops', 'DevOps', true,
    1759136400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-29', 'roundups', 'fabric', 'Fabric', false,
    1759136400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-29', 'roundups', 'github', 'GitHub', false,
    1759136400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-29', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1759136400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-29', 'roundups', 'hybrid', 'Hybrid', false,
    1759136400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-29', 'roundups', 'hybrid cloud', 'Hybrid Cloud', true,
    1759136400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-29', 'roundups', 'iac', 'IaC', true,
    1759136400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-29', 'roundups', 'mcp', 'MCP', true,
    1759136400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-29', 'roundups', 'microsoft', 'Microsoft', false,
    1759136400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-29', 'roundups', 'microsoft fabric', 'Microsoft Fabric', true,
    1759136400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-29', 'roundups', 'ml', 'ML', true,
    1759136400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-29', 'roundups', 'mlops', 'MLOps', true,
    1759136400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-29', 'roundups', 'modernization', 'Modernization', false,
    1759136400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-29', 'roundups', 'openai', 'OpenAI', true,
    1759136400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-29', 'roundups', 'opus', 'Opus', false,
    1759136400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-29', 'roundups', 'roundups', 'Roundups', true,
    1759136400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-29', 'roundups', 'security', 'Security', false,
    1759136400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-29', 'roundups', 'supply', 'Supply', false,
    1759136400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-29', 'roundups', 'supply chain security', 'Supply Chain Security', true,
    1759136400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-09-29', 'roundups', 'vs', 'VS', true,
    1759136400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-06', 'roundups', '.net', '.NET', true,
    1759741200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-06', 'roundups', 'agents', 'Agents', false,
    1759741200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-06', 'roundups', 'ai', 'AI', false,
    1759741200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-06', 'roundups', 'ai agents', 'AI Agents', true,
    1759741200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-06', 'roundups', 'automation', 'Automation', true,
    1759741200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-06', 'roundups', 'azure', 'Azure', true,
    1759741200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-06', 'roundups', 'cloud', 'Cloud', false,
    1759741200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-06', 'roundups', 'coding', 'Coding', false,
    1759741200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-06', 'roundups', 'coding agents', 'Coding Agents', true,
    1759741200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-06', 'roundups', 'copilot', 'Copilot', false,
    1759741200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-06', 'roundups', 'data', 'Data', false,
    1759741200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-06', 'roundups', 'data transformation', 'Data Transformation', true,
    1759741200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-06', 'roundups', 'devops', 'DevOps', true,
    1759741200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-06', 'roundups', 'enterprise', 'Enterprise', false,
    1759741200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-06', 'roundups', 'enterprise cloud', 'Enterprise Cloud', true,
    1759741200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-06', 'roundups', 'fabric', 'Fabric', false,
    1759741200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-06', 'roundups', 'github', 'GitHub', false,
    1759741200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-06', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1759741200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-06', 'roundups', 'mcp', 'MCP', true,
    1759741200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-06', 'roundups', 'microsoft', 'Microsoft', false,
    1759741200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-06', 'roundups', 'microsoft fabric', 'Microsoft Fabric', true,
    1759741200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-06', 'roundups', 'ml', 'ML', true,
    1759741200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-06', 'roundups', 'multicloud', 'Multicloud', true,
    1759741200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-06', 'roundups', 'orchestration', 'Orchestration', false,
    1759741200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-06', 'roundups', 'roundups', 'Roundups', true,
    1759741200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-06', 'roundups', 'security', 'Security', true,
    1759741200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-06', 'roundups', 'transformation', 'Transformation', false,
    1759741200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-06', 'roundups', 'vs', 'VS', true,
    1759741200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-06', 'roundups', 'workflow', 'Workflow', false,
    1759741200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-06', 'roundups', 'workflow orchestration', 'Workflow Orchestration', true,
    1759741200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-13', 'roundups', '.net', '.NET', true,
    1760346000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-13', 'roundups', 'agents', 'Agents', false,
    1760346000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-13', 'roundups', 'ai', 'AI', false,
    1760346000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-13', 'roundups', 'ai agents', 'AI Agents', true,
    1760346000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-13', 'roundups', 'azure', 'Azure', false,
    1760346000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-13', 'roundups', 'azure ai', 'Azure AI', true,
    1760346000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-13', 'roundups', 'ci/cd', 'CI/CD', true,
    1760346000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-13', 'roundups', 'cloud', 'Cloud', false,
    1760346000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-13', 'roundups', 'cloud infrastructure', 'Cloud Infrastructure', true,
    1760346000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-13', 'roundups', 'computing', 'Computing', false,
    1760346000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-13', 'roundups', 'copilot', 'Copilot', false,
    1760346000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-13', 'roundups', 'data', 'Data', false,
    1760346000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-13', 'roundups', 'data engineering', 'Data Engineering', true,
    1760346000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-13', 'roundups', 'developer', 'Developer', false,
    1760346000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-13', 'roundups', 'developer tools', 'Developer Tools', true,
    1760346000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-13', 'roundups', 'devops', 'DevOps', true,
    1760346000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-13', 'roundups', 'engineering', 'Engineering', false,
    1760346000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-13', 'roundups', 'github', 'GitHub', false,
    1760346000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-13', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1760346000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-13', 'roundups', 'infrastructure', 'Infrastructure', false,
    1760346000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-13', 'roundups', 'ml', 'ML', true,
    1760346000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-13', 'roundups', 'models', 'Models', false,
    1760346000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-13', 'roundups', 'multimodal', 'Multimodal', false,
    1760346000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-13', 'roundups', 'multimodal models', 'Multimodal Models', true,
    1760346000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-13', 'roundups', 'observability', 'Observability', true,
    1760346000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-13', 'roundups', 'open', 'Open', false,
    1760346000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-13', 'roundups', 'open source', 'Open Source', true,
    1760346000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-13', 'roundups', 'roundups', 'Roundups', true,
    1760346000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-13', 'roundups', 'security', 'Security', true,
    1760346000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-13', 'roundups', 'serverless', 'Serverless', false,
    1760346000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-13', 'roundups', 'serverless computing', 'Serverless Computing', true,
    1760346000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-13', 'roundups', 'source', 'Source', false,
    1760346000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-13', 'roundups', 'tools', 'Tools', false,
    1760346000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-13', 'roundups', 'trust', 'Trust', false,
    1760346000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-13', 'roundups', 'zero', 'Zero', false,
    1760346000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-13', 'roundups', 'zero trust', 'Zero Trust', true,
    1760346000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-20', 'roundups', '.net', '.NET', true,
    1760950800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-20', 'roundups', 'agentic', 'Agentic', false,
    1760950800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-20', 'roundups', 'agentic ai', 'Agentic AI', true,
    1760950800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-20', 'roundups', 'ai', 'AI', false,
    1760950800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-20', 'roundups', 'automation', 'Automation', true,
    1760950800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-20', 'roundups', 'azure', 'Azure', true,
    1760950800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-20', 'roundups', 'cloud', 'Cloud', false,
    1760950800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-20', 'roundups', 'cloud infrastructure', 'Cloud Infrastructure', true,
    1760950800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-20', 'roundups', 'code', 'Code', false,
    1760950800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-20', 'roundups', 'copilot', 'Copilot', false,
    1760950800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-20', 'roundups', 'developer', 'Developer', false,
    1760950800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-20', 'roundups', 'developer workflows', 'Developer Workflows', true,
    1760950800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-20', 'roundups', 'devops', 'DevOps', true,
    1760950800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-20', 'roundups', 'github', 'GitHub', false,
    1760950800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-20', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1760950800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-20', 'roundups', 'hardware', 'Hardware', false,
    1760950800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-20', 'roundups', 'infrastructure', 'Infrastructure', false,
    1760950800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-20', 'roundups', 'mcp', 'MCP', true,
    1760950800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-20', 'roundups', 'ml', 'ML', true,
    1760950800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-20', 'roundups', 'open', 'Open', false,
    1760950800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-20', 'roundups', 'open source', 'Open Source', true,
    1760950800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-20', 'roundups', 'quantum', 'Quantum', false,
    1760950800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-20', 'roundups', 'quantum resilient hardware', 'Quantum Resilient Hardware', true,
    1760950800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-20', 'roundups', 'resilient', 'Resilient', false,
    1760950800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-20', 'roundups', 'roundups', 'Roundups', true,
    1760950800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-20', 'roundups', 'security', 'Security', true,
    1760950800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-20', 'roundups', 'source', 'Source', false,
    1760950800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-20', 'roundups', 'vs', 'VS', false,
    1760950800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-20', 'roundups', 'vs code', 'VS Code', true,
    1760950800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-20', 'roundups', 'workflows', 'Workflows', false,
    1760950800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-27', 'roundups', '.net', '.NET', true,
    1761555600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-27', 'roundups', 'agent', 'Agent', false,
    1761555600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-27', 'roundups', 'agent framework', 'Agent Framework', true,
    1761555600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-27', 'roundups', 'agents', 'Agents', false,
    1761555600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-27', 'roundups', 'ai', 'AI', false,
    1761555600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-27', 'roundups', 'ai agents', 'AI Agents', true,
    1761555600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-27', 'roundups', 'augmented', 'Augmented', false,
    1761555600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-27', 'roundups', 'azure', 'Azure', true,
    1761555600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-27', 'roundups', 'chain', 'Chain', false,
    1761555600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-27', 'roundups', 'cloud', 'Cloud', false,
    1761555600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-27', 'roundups', 'cloud native', 'Cloud Native', true,
    1761555600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-27', 'roundups', 'code', 'Code', false,
    1761555600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-27', 'roundups', 'copilot', 'Copilot', false,
    1761555600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-27', 'roundups', 'copilot studio', 'Copilot Studio', true,
    1761555600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-27', 'roundups', 'devops', 'DevOps', true,
    1761555600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-27', 'roundups', 'framework', 'Framework', false,
    1761555600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-27', 'roundups', 'generation', 'Generation', false,
    1761555600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-27', 'roundups', 'github', 'GitHub', false,
    1761555600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-27', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1761555600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-27', 'roundups', 'hybrid', 'Hybrid', false,
    1761555600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-27', 'roundups', 'hybrid cloud', 'Hybrid Cloud', true,
    1761555600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-27', 'roundups', 'ml', 'ML', true,
    1761555600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-27', 'roundups', 'native', 'Native', false,
    1761555600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-27', 'roundups', 'observability', 'Observability', true,
    1761555600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-27', 'roundups', 'opentelemetry', 'OpenTelemetry', true,
    1761555600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-27', 'roundups', 'retrieval', 'Retrieval', false,
    1761555600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-27', 'roundups', 'retrieval augmented generation', 'Retrieval Augmented Generation', true,
    1761555600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-27', 'roundups', 'roundups', 'Roundups', true,
    1761555600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-27', 'roundups', 'security', 'Security', false,
    1761555600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-27', 'roundups', 'studio', 'Studio', false,
    1761555600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-27', 'roundups', 'supply', 'Supply', false,
    1761555600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-27', 'roundups', 'supply chain security', 'Supply Chain Security', true,
    1761555600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-27', 'roundups', 'vs', 'VS', false,
    1761555600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-10-27', 'roundups', 'vs code', 'VS Code', true,
    1761555600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-03', 'roundups', '.net', '.NET', true,
    1762160400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-03', 'roundups', 'agents', 'Agents', false,
    1762160400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-03', 'roundups', 'ai', 'AI', false,
    1762160400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-03', 'roundups', 'ai agents', 'AI Agents', true,
    1762160400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-03', 'roundups', 'ai security', 'AI Security', true,
    1762160400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-03', 'roundups', 'app', 'App', false,
    1762160400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-03', 'roundups', 'app modernization', 'App Modernization', true,
    1762160400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-03', 'roundups', 'azure', 'Azure', true,
    1762160400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-03', 'roundups', 'code', 'Code', false,
    1762160400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-03', 'roundups', 'computing', 'Computing', false,
    1762160400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-03', 'roundups', 'copilot', 'Copilot', false,
    1762160400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-03', 'roundups', 'developer', 'Developer', false,
    1762160400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-03', 'roundups', 'developer tools', 'Developer Tools', true,
    1762160400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-03', 'roundups', 'devops', 'DevOps', true,
    1762160400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-03', 'roundups', 'fabric', 'Fabric', false,
    1762160400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-03', 'roundups', 'generative', 'Generative', false,
    1762160400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-03', 'roundups', 'generative ai', 'Generative AI', true,
    1762160400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-03', 'roundups', 'github', 'GitHub', false,
    1762160400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-03', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1762160400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-03', 'roundups', 'microsoft', 'Microsoft', false,
    1762160400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-03', 'roundups', 'microsoft fabric', 'Microsoft Fabric', true,
    1762160400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-03', 'roundups', 'ml', 'ML', true,
    1762160400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-03', 'roundups', 'modernization', 'Modernization', false,
    1762160400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-03', 'roundups', 'open', 'Open', false,
    1762160400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-03', 'roundups', 'open source', 'Open Source', true,
    1762160400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-03', 'roundups', 'quantum', 'Quantum', false,
    1762160400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-03', 'roundups', 'quantum computing', 'Quantum Computing', true,
    1762160400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-03', 'roundups', 'roundups', 'Roundups', true,
    1762160400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-03', 'roundups', 'search', 'Search', false,
    1762160400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-03', 'roundups', 'security', 'Security', false,
    1762160400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-03', 'roundups', 'source', 'Source', false,
    1762160400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-03', 'roundups', 'tools', 'Tools', false,
    1762160400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-03', 'roundups', 'vector', 'Vector', false,
    1762160400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-03', 'roundups', 'vector search', 'Vector Search', true,
    1762160400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-03', 'roundups', 'vs', 'VS', false,
    1762160400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-03', 'roundups', 'vs code', 'VS Code', true,
    1762160400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-10', 'roundups', '.net', '.NET', true,
    1762765200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-10', 'roundups', 'agents', 'Agents', false,
    1762765200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-10', 'roundups', 'ai', 'AI', false,
    1762765200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-10', 'roundups', 'ai agents', 'AI Agents', true,
    1762765200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-10', 'roundups', 'automation', 'Automation', false,
    1762765200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-10', 'roundups', 'azure', 'Azure', true,
    1762765200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-10', 'roundups', 'ci/cd', 'CI/CD', true,
    1762765200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-10', 'roundups', 'cloud', 'Cloud', false,
    1762765200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-10', 'roundups', 'cloud native', 'Cloud Native', true,
    1762765200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-10', 'roundups', 'containerization', 'Containerization', true,
    1762765200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-10', 'roundups', 'copilot', 'Copilot', false,
    1762765200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-10', 'roundups', 'devops', 'DevOps', true,
    1762765200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-10', 'roundups', 'enterprise', 'Enterprise', false,
    1762765200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-10', 'roundups', 'enterprise ai', 'Enterprise AI', true,
    1762765200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-10', 'roundups', 'fabric', 'Fabric', false,
    1762765200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-10', 'roundups', 'github', 'GitHub', false,
    1762765200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-10', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1762765200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-10', 'roundups', 'kubernetes', 'Kubernetes', true,
    1762765200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-10', 'roundups', 'microsoft', 'Microsoft', false,
    1762765200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-10', 'roundups', 'microsoft fabric', 'Microsoft Fabric', true,
    1762765200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-10', 'roundups', 'ml', 'ML', true,
    1762765200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-10', 'roundups', 'native', 'Native', false,
    1762765200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-10', 'roundups', 'roundups', 'Roundups', true,
    1762765200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-10', 'roundups', 'security', 'Security', true,
    1762765200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-10', 'roundups', 'typescript', 'TypeScript', true,
    1762765200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-10', 'roundups', 'workflow', 'Workflow', false,
    1762765200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-10', 'roundups', 'workflow automation', 'Workflow Automation', true,
    1762765200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-17', 'roundups', '.net', '.NET', false,
    1763370000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-17', 'roundups', '.net 10', '.NET 10', true,
    1763370000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-17', 'roundups', '10', '10', false,
    1763370000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-17', 'roundups', 'agents', 'Agents', false,
    1763370000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-17', 'roundups', 'ai', 'AI', false,
    1763370000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-17', 'roundups', 'ai agents', 'AI Agents', true,
    1763370000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-17', 'roundups', 'automation', 'Automation', false,
    1763370000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-17', 'roundups', 'azure', 'Azure', true,
    1763370000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-17', 'roundups', 'cloud', 'Cloud', false,
    1763370000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-17', 'roundups', 'cloud native', 'Cloud Native', true,
    1763370000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-17', 'roundups', 'code', 'Code', false,
    1763370000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-17', 'roundups', 'code review', 'Code Review', true,
    1763370000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-17', 'roundups', 'copilot', 'Copilot', false,
    1763370000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-17', 'roundups', 'devops', 'DevOps', true,
    1763370000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-17', 'roundups', 'engineering', 'Engineering', false,
    1763370000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-17', 'roundups', 'enterprise', 'Enterprise', false,
    1763370000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-17', 'roundups', 'enterprise automation', 'Enterprise Automation', true,
    1763370000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-17', 'roundups', 'github', 'GitHub', false,
    1763370000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-17', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1763370000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-17', 'roundups', 'kubernetes', 'Kubernetes', true,
    1763370000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-17', 'roundups', 'mcp', 'MCP', true,
    1763370000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-17', 'roundups', 'ml', 'ML', true,
    1763370000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-17', 'roundups', 'native', 'Native', false,
    1763370000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-17', 'roundups', 'observability', 'Observability', true,
    1763370000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-17', 'roundups', 'prompt', 'Prompt', false,
    1763370000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-17', 'roundups', 'prompt engineering', 'Prompt Engineering', true,
    1763370000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-17', 'roundups', 'review', 'Review', false,
    1763370000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-17', 'roundups', 'roundups', 'Roundups', true,
    1763370000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-17', 'roundups', 'security', 'Security', true,
    1763370000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-17', 'roundups', 'vs', 'VS', true,
    1763370000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-24', 'roundups', '.net', '.NET', true,
    1763974800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-24', 'roundups', 'agents', 'Agents', false,
    1763974800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-24', 'roundups', 'ai', 'AI', false,
    1763974800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-24', 'roundups', 'ai agents', 'AI Agents', true,
    1763974800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-24', 'roundups', 'automation', 'Automation', false,
    1763974800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-24', 'roundups', 'azure', 'Azure', true,
    1763974800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-24', 'roundups', 'cloud', 'Cloud', false,
    1763974800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-24', 'roundups', 'cloud infrastructure', 'Cloud Infrastructure', true,
    1763974800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-24', 'roundups', 'code', 'Code', false,
    1763974800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-24', 'roundups', 'copilot', 'Copilot', false,
    1763974800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-24', 'roundups', 'cryptography', 'Cryptography', false,
    1763974800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-24', 'roundups', 'data', 'Data', false,
    1763974800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-24', 'roundups', 'data governance', 'Data Governance', true,
    1763974800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-24', 'roundups', 'deployment', 'Deployment', false,
    1763974800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-24', 'roundups', 'devops', 'DevOps', true,
    1763974800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-24', 'roundups', 'github', 'GitHub', false,
    1763974800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-24', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1763974800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-24', 'roundups', 'governance', 'Governance', false,
    1763974800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-24', 'roundups', 'ide', 'IDE', false,
    1763974800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-24', 'roundups', 'ide integration', 'IDE Integration', true,
    1763974800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-24', 'roundups', 'infrastructure', 'Infrastructure', false,
    1763974800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-24', 'roundups', 'integration', 'Integration', false,
    1763974800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-24', 'roundups', 'ml', 'ML', true,
    1763974800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-24', 'roundups', 'model', 'Model', false,
    1763974800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-24', 'roundups', 'model deployment', 'Model Deployment', true,
    1763974800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-24', 'roundups', 'post', 'Post', false,
    1763974800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-24', 'roundups', 'post quantum cryptography', 'Post Quantum Cryptography', true,
    1763974800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-24', 'roundups', 'quantum', 'Quantum', false,
    1763974800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-24', 'roundups', 'roundups', 'Roundups', true,
    1763974800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-24', 'roundups', 'security', 'Security', true,
    1763974800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-24', 'roundups', 'vs', 'VS', true,
    1763974800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-24', 'roundups', 'vs code', 'VS Code', true,
    1763974800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-24', 'roundups', 'workflow', 'Workflow', false,
    1763974800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-11-24', 'roundups', 'workflow automation', 'Workflow Automation', true,
    1763974800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', '.net', '.NET', true,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'agentic', 'Agentic', false,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'agentic ai', 'Agentic AI', true,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'ai', 'AI', false,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'automation', 'Automation', true,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'azure', 'Azure', true,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'cloud', 'Cloud', false,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'cloud native', 'Cloud Native', true,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'code', 'Code', false,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'computing', 'Computing', false,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'confidential', 'Confidential', false,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'confidential computing', 'Confidential Computing', true,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'copilot', 'Copilot', false,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'data', 'Data', false,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'data platform', 'Data Platform', true,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'developer', 'Developer', false,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'developer productivity', 'Developer Productivity', true,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'device', 'Device', false,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'device security', 'Device Security', true,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'devops', 'DevOps', true,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'enterprise', 'Enterprise', false,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'enterprise migration', 'Enterprise Migration', true,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'foundry', 'Foundry', false,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'github', 'GitHub', false,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'low', 'Low', false,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'low code', 'Low Code', true,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'microsoft', 'Microsoft', false,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'microsoft foundry', 'Microsoft Foundry', true,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'migration', 'Migration', false,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'ml', 'ML', true,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'native', 'Native', false,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'observability', 'Observability', true,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'platform', 'Platform', false,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'power', 'Power', false,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'power platform', 'Power Platform', true,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'productivity', 'Productivity', false,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'roundups', 'Roundups', true,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'security', 'Security', false,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'trust', 'Trust', false,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'vs', 'VS', true,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'zero', 'Zero', false,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-01', 'roundups', 'zero trust', 'Zero Trust', true,
    1764579600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-08', 'roundups', '.net', '.NET', false,
    1765184400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-08', 'roundups', '.net 10', '.NET 10', true,
    1765184400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-08', 'roundups', '10', '10', false,
    1765184400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-08', 'roundups', 'agent', 'Agent', false,
    1765184400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-08', 'roundups', 'agent framework', 'Agent Framework', true,
    1765184400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-08', 'roundups', 'agents', 'Agents', false,
    1765184400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-08', 'roundups', 'ai', 'AI', false,
    1765184400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-08', 'roundups', 'ai agents', 'AI Agents', true,
    1765184400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-08', 'roundups', 'automation', 'Automation', true,
    1765184400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-08', 'roundups', 'azure', 'Azure', false,
    1765184400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-08', 'roundups', 'cloud', 'Cloud', false,
    1765184400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-08', 'roundups', 'cloud native', 'Cloud Native', true,
    1765184400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-08', 'roundups', 'copilot', 'Copilot', false,
    1765184400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-08', 'roundups', 'devops', 'DevOps', true,
    1765184400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-08', 'roundups', 'enterprise', 'Enterprise', false,
    1765184400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-08', 'roundups', 'enterprise ai', 'Enterprise AI', true,
    1765184400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-08', 'roundups', 'fabric', 'Fabric', false,
    1765184400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-08', 'roundups', 'framework', 'Framework', false,
    1765184400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-08', 'roundups', 'github', 'GitHub', false,
    1765184400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-08', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1765184400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-08', 'roundups', 'governance', 'Governance', true,
    1765184400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-08', 'roundups', 'microsoft', 'Microsoft', false,
    1765184400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-08', 'roundups', 'microsoft azure', 'Microsoft Azure', true,
    1765184400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-08', 'roundups', 'microsoft fabric', 'Microsoft Fabric', true,
    1765184400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-08', 'roundups', 'ml', 'ML', true,
    1765184400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-08', 'roundups', 'native', 'Native', false,
    1765184400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-08', 'roundups', 'openai', 'OpenAI', true,
    1765184400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-08', 'roundups', 'roundups', 'Roundups', true,
    1765184400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-08', 'roundups', 'security', 'Security', true,
    1765184400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-08', 'roundups', 'typescript', 'TypeScript', true,
    1765184400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-08', 'roundups', 'vs', 'VS', true,
    1765184400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-15', 'roundups', '.net', '.NET', true,
    1765789200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-15', 'roundups', '5.2', '5.2', false,
    1765789200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-15', 'roundups', 'agentic', 'Agentic', false,
    1765789200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-15', 'roundups', 'agentic ai', 'Agentic AI', true,
    1765789200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-15', 'roundups', 'agents', 'Agents', false,
    1765789200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-15', 'roundups', 'ai', 'AI', false,
    1765789200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-15', 'roundups', 'ai agents', 'AI Agents', true,
    1765789200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-15', 'roundups', 'application', 'Application', false,
    1765789200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-15', 'roundups', 'application modernization', 'Application Modernization', true,
    1765789200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-15', 'roundups', 'automation', 'Automation', false,
    1765789200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-15', 'roundups', 'azure', 'Azure', true,
    1765789200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-15', 'roundups', 'chain', 'Chain', false,
    1765789200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-15', 'roundups', 'ci/cd', 'CI/CD', true,
    1765789200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-15', 'roundups', 'cloud', 'Cloud', false,
    1765789200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-15', 'roundups', 'cloud automation', 'Cloud Automation', true,
    1765789200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-15', 'roundups', 'code', 'Code', false,
    1765789200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-15', 'roundups', 'copilot', 'Copilot', false,
    1765789200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-15', 'roundups', 'developer', 'Developer', false,
    1765789200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-15', 'roundups', 'developer productivity', 'Developer Productivity', true,
    1765789200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-15', 'roundups', 'devops', 'DevOps', true,
    1765789200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-15', 'roundups', 'fabric', 'Fabric', false,
    1765789200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-15', 'roundups', 'github', 'GitHub', false,
    1765789200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-15', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1765789200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-15', 'roundups', 'gpt', 'GPT', false,
    1765789200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-15', 'roundups', 'microsoft', 'Microsoft', false,
    1765789200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-15', 'roundups', 'microsoft fabric', 'Microsoft Fabric', true,
    1765789200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-15', 'roundups', 'ml', 'ML', true,
    1765789200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-15', 'roundups', 'modernization', 'Modernization', false,
    1765789200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-15', 'roundups', 'openai', 'OpenAI', false,
    1765789200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-15', 'roundups', 'openai gpt 5.2', 'OpenAI GPT 5.2', true,
    1765789200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-15', 'roundups', 'productivity', 'Productivity', false,
    1765789200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-15', 'roundups', 'roundups', 'Roundups', true,
    1765789200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-15', 'roundups', 'security', 'Security', false,
    1765789200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-15', 'roundups', 'supply', 'Supply', false,
    1765789200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-15', 'roundups', 'supply chain security', 'Supply Chain Security', true,
    1765789200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-15', 'roundups', 'vs', 'VS', false,
    1765789200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-15', 'roundups', 'vs code', 'VS Code', true,
    1765789200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-22', 'roundups', '.net', '.NET', true,
    1766394000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-22', 'roundups', 'ai', 'AI', true,
    1766394000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-22', 'roundups', 'automation', 'Automation', true,
    1766394000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-22', 'roundups', 'azure', 'Azure', true,
    1766394000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-22', 'roundups', 'cloud', 'Cloud', false,
    1766394000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-22', 'roundups', 'cloud computing', 'Cloud Computing', true,
    1766394000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-22', 'roundups', 'code', 'Code', false,
    1766394000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-22', 'roundups', 'code review', 'Code Review', true,
    1766394000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-22', 'roundups', 'computing', 'Computing', false,
    1766394000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-22', 'roundups', 'copilot', 'Copilot', false,
    1766394000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-22', 'roundups', 'data', 'Data', false,
    1766394000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-22', 'roundups', 'data engineering', 'Data Engineering', true,
    1766394000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-22', 'roundups', 'dependabot', 'Dependabot', true,
    1766394000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-22', 'roundups', 'devops', 'DevOps', true,
    1766394000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-22', 'roundups', 'engineering', 'Engineering', false,
    1766394000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-22', 'roundups', 'fabric', 'Fabric', false,
    1766394000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-22', 'roundups', 'github', 'GitHub', false,
    1766394000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-22', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1766394000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-22', 'roundups', 'microsoft', 'Microsoft', false,
    1766394000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-22', 'roundups', 'microsoft fabric', 'Microsoft Fabric', true,
    1766394000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-22', 'roundups', 'ml', 'ML', true,
    1766394000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-22', 'roundups', 'orchestration', 'Orchestration', true,
    1766394000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-22', 'roundups', 'prompt', 'Prompt', false,
    1766394000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-22', 'roundups', 'prompt engineering', 'Prompt Engineering', true,
    1766394000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-22', 'roundups', 'review', 'Review', false,
    1766394000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-22', 'roundups', 'roundups', 'Roundups', true,
    1766394000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-22', 'roundups', 'security', 'Security', true,
    1766394000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-22', 'roundups', 'vs', 'VS', false,
    1766394000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-22', 'roundups', 'vs code', 'VS Code', true,
    1766394000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-29', 'roundups', '(model', '(model', false,
    1766998800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-29', 'roundups', '.net', '.NET', true,
    1766998800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-29', 'roundups', 'agent', 'Agent', false,
    1766998800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-29', 'roundups', 'agent framework', 'Agent Framework', true,
    1766998800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-29', 'roundups', 'agents', 'Agents', false,
    1766998800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-29', 'roundups', 'ai', 'AI', false,
    1766998800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-29', 'roundups', 'ai agents', 'AI Agents', true,
    1766998800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-29', 'roundups', 'automation', 'Automation', true,
    1766998800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-29', 'roundups', 'azure', 'Azure', false,
    1766998800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-29', 'roundups', 'azure databricks', 'Azure Databricks', true,
    1766998800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-29', 'roundups', 'azure sre', 'Azure SRE', true,
    1766998800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-29', 'roundups', 'chain', 'Chain', false,
    1766998800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-29', 'roundups', 'ci/cd', 'CI/CD', true,
    1766998800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-29', 'roundups', 'code', 'Code', false,
    1766998800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-29', 'roundups', 'context', 'Context', false,
    1766998800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-29', 'roundups', 'copilot', 'Copilot', false,
    1766998800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-29', 'roundups', 'databricks', 'Databricks', false,
    1766998800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-29', 'roundups', 'devops', 'DevOps', true,
    1766998800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-29', 'roundups', 'framework', 'Framework', false,
    1766998800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-29', 'roundups', 'github', 'GitHub', false,
    1766998800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-29', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1766998800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-29', 'roundups', 'load', 'Load', false,
    1766998800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-29', 'roundups', 'load testing', 'Load Testing', true,
    1766998800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-29', 'roundups', 'mcp', 'MCP', false,
    1766998800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-29', 'roundups', 'mcp (model context protocol)', 'MCP (model Context Protocol)', true,
    1766998800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-29', 'roundups', 'ml', 'ML', true,
    1766998800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-29', 'roundups', 'open', 'Open', false,
    1766998800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-29', 'roundups', 'open source', 'Open Source', true,
    1766998800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-29', 'roundups', 'protocol)', 'Protocol)', false,
    1766998800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-29', 'roundups', 'roundups', 'Roundups', true,
    1766998800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-29', 'roundups', 'security', 'Security', false,
    1766998800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-29', 'roundups', 'source', 'Source', false,
    1766998800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-29', 'roundups', 'sre', 'SRE', false,
    1766998800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-29', 'roundups', 'supply', 'Supply', false,
    1766998800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-29', 'roundups', 'supply chain security', 'Supply Chain Security', true,
    1766998800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-29', 'roundups', 'testing', 'Testing', false,
    1766998800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-29', 'roundups', 'vs', 'VS', false,
    1766998800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2025-12-29', 'roundups', 'vs code', 'VS Code', true,
    1766998800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', '.net', '.NET', false,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', '.net 10', '.NET 10', true,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', '10', '10', false,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'account', 'Account', false,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'account recovery', 'Account Recovery', true,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'agent', 'Agent', false,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'agentic', 'Agentic', false,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'agentic ai', 'Agentic AI', true,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'ai', 'AI', false,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'ai integrations', 'AI Integrations', true,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'automation', 'Automation', false,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'azure', 'Azure', true,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'ci/cd', 'CI/CD', true,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'cloud', 'Cloud', false,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'cloud automation', 'Cloud Automation', true,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'code', 'Code', false,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'copilot', 'Copilot', false,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'data', 'Data', false,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'data migration', 'Data Migration', true,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'developer', 'Developer', false,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'developer workflows', 'Developer Workflows', true,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'devops', 'DevOps', true,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'framework', 'Framework', false,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'fuzz', 'Fuzz', false,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'fuzz testing', 'Fuzz Testing', true,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'github', 'GitHub', false,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'integrations', 'Integrations', false,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'mcp', 'MCP', true,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'microsoft', 'Microsoft', false,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'microsoft agent framework', 'Microsoft Agent Framework', true,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'migration', 'Migration', false,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'ml', 'ML', true,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'modeling', 'Modeling', false,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'recovery', 'Recovery', false,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'roundups', 'Roundups', true,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'security', 'Security', true,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'semantic', 'Semantic', false,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'semantic modeling', 'Semantic Modeling', true,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'testing', 'Testing', false,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'vs', 'VS', false,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'vs code', 'VS Code', true,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-05', 'roundups', 'workflows', 'Workflows', false,
    1767603600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', '.net', '.NET', true,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'agent', 'Agent', false,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'agentic', 'Agentic', false,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'agentic ai', 'Agentic AI', true,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'ai', 'AI', false,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'ai driven automation', 'AI Driven Automation', true,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'automation', 'Automation', false,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'azure', 'Azure', true,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'c#', 'C#', true,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'cloud', 'Cloud', false,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'cloud cost management', 'Cloud Cost Management', true,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'cloud security', 'Cloud Security', true,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'computing', 'Computing', false,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'copilot', 'Copilot', false,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'cost', 'Cost', false,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'devops', 'DevOps', true,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'driven', 'Driven', false,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'fabric', 'Fabric', false,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'github', 'GitHub', false,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'high', 'High', false,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'high performance computing', 'High Performance Computing', true,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'ide', 'IDE', false,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'ide integration', 'IDE Integration', true,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'integration', 'Integration', false,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'management', 'Management', false,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'microsoft', 'Microsoft', false,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'microsoft fabric', 'Microsoft Fabric', true,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'ml', 'ML', true,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'multi', 'Multi', false,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'multi agent orchestration', 'Multi Agent Orchestration', true,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'open', 'Open', false,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'open source', 'Open Source', true,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'orchestration', 'Orchestration', false,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'performance', 'Performance', false,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'roundups', 'Roundups', true,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'search', 'Search', false,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'security', 'Security', false,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'semantic', 'Semantic', false,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'semantic search', 'Semantic Search', true,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-12', 'roundups', 'source', 'Source', false,
    1768208400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-19', 'roundups', '.net', '.NET', true,
    1768813200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-19', 'roundups', 'access', 'Access', false,
    1768813200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-19', 'roundups', 'agentic', 'Agentic', false,
    1768813200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-19', 'roundups', 'agentic ai', 'Agentic AI', true,
    1768813200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-19', 'roundups', 'ai', 'AI', false,
    1768813200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-19', 'roundups', 'and', 'And', false,
    1768813200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-19', 'roundups', 'anthropic', 'Anthropic', true,
    1768813200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-19', 'roundups', 'azure', 'Azure', false,
    1768813200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-19', 'roundups', 'cloud', 'Cloud', false,
    1768813200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-19', 'roundups', 'cloud infrastructure', 'Cloud Infrastructure', true,
    1768813200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-19', 'roundups', 'code', 'Code', false,
    1768813200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-19', 'roundups', 'containerization', 'Containerization', true,
    1768813200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-19', 'roundups', 'copilot', 'Copilot', false,
    1768813200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-19', 'roundups', 'devops', 'DevOps', true,
    1768813200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-19', 'roundups', 'enterprise', 'Enterprise', true,
    1768813200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-19', 'roundups', 'github', 'GitHub', false,
    1768813200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-19', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1768813200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-19', 'roundups', 'identity', 'Identity', false,
    1768813200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-19', 'roundups', 'identity and access management', 'Identity And Access Management', true,
    1768813200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-19', 'roundups', 'infrastructure', 'Infrastructure', false,
    1768813200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-19', 'roundups', 'management', 'Management', false,
    1768813200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-19', 'roundups', 'microsoft', 'Microsoft', false,
    1768813200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-19', 'roundups', 'microsoft azure', 'Microsoft Azure', true,
    1768813200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-19', 'roundups', 'ml', 'ML', true,
    1768813200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-19', 'roundups', 'openai', 'OpenAI', true,
    1768813200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-19', 'roundups', 'roundups', 'Roundups', true,
    1768813200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-19', 'roundups', 'security', 'Security', true,
    1768813200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-19', 'roundups', 'vs', 'VS', false,
    1768813200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-19', 'roundups', 'vs code', 'VS Code', true,
    1768813200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-26', 'roundups', '.net', '.NET', true,
    1769418000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-26', 'roundups', 'agentic', 'Agentic', false,
    1769418000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-26', 'roundups', 'agentic sdk', 'Agentic SDK', true,
    1769418000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-26', 'roundups', 'ai', 'AI', true,
    1769418000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-26', 'roundups', 'app', 'App', false,
    1769418000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-26', 'roundups', 'app modernization', 'App Modernization', true,
    1769418000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-26', 'roundups', 'automation', 'Automation', false,
    1769418000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-26', 'roundups', 'azure', 'Azure', true,
    1769418000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-26', 'roundups', 'ci/cd', 'CI/CD', true,
    1769418000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-26', 'roundups', 'cloud', 'Cloud', false,
    1769418000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-26', 'roundups', 'cloud engineering', 'Cloud Engineering', true,
    1769418000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-26', 'roundups', 'copilot', 'Copilot', false,
    1769418000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-26', 'roundups', 'developer', 'Developer', false,
    1769418000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-26', 'roundups', 'developer tools', 'Developer Tools', true,
    1769418000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-26', 'roundups', 'devops', 'DevOps', true,
    1769418000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-26', 'roundups', 'engineering', 'Engineering', false,
    1769418000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-26', 'roundups', 'fabric', 'Fabric', false,
    1769418000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-26', 'roundups', 'github', 'GitHub', false,
    1769418000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-26', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1769418000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-26', 'roundups', 'kubernetes', 'Kubernetes', true,
    1769418000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-26', 'roundups', 'microsoft', 'Microsoft', false,
    1769418000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-26', 'roundups', 'microsoft fabric', 'Microsoft Fabric', true,
    1769418000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-26', 'roundups', 'ml', 'ML', true,
    1769418000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-26', 'roundups', 'modernization', 'Modernization', false,
    1769418000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-26', 'roundups', 'open', 'Open', false,
    1769418000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-26', 'roundups', 'open source', 'Open Source', true,
    1769418000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-26', 'roundups', 'roundups', 'Roundups', true,
    1769418000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-26', 'roundups', 'sdk', 'SDK', false,
    1769418000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-26', 'roundups', 'security', 'Security', true,
    1769418000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-26', 'roundups', 'source', 'Source', false,
    1769418000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-26', 'roundups', 'tools', 'Tools', false,
    1769418000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-26', 'roundups', 'workflow', 'Workflow', false,
    1769418000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-01-26', 'roundups', 'workflow automation', 'Workflow Automation', true,
    1769418000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-02', 'roundups', '.net', '.NET', true,
    1770022800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-02', 'roundups', 'agentic', 'Agentic', false,
    1770022800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-02', 'roundups', 'agentic workflows', 'Agentic Workflows', true,
    1770022800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-02', 'roundups', 'ai', 'AI', true,
    1770022800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-02', 'roundups', 'automation', 'Automation', false,
    1770022800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-02', 'roundups', 'azure', 'Azure', true,
    1770022800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-02', 'roundups', 'cli', 'CLI', false,
    1770022800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-02', 'roundups', 'cli automation', 'CLI Automation', true,
    1770022800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-02', 'roundups', 'cloud', 'Cloud', false,
    1770022800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-02', 'roundups', 'cloud computing', 'Cloud Computing', true,
    1770022800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-02', 'roundups', 'compliance', 'Compliance', true,
    1770022800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-02', 'roundups', 'computing', 'Computing', false,
    1770022800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-02', 'roundups', 'copilot', 'Copilot', false,
    1770022800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-02', 'roundups', 'data', 'Data', false,
    1770022800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-02', 'roundups', 'data engineering', 'Data Engineering', true,
    1770022800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-02', 'roundups', 'devops', 'DevOps', true,
    1770022800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-02', 'roundups', 'engineering', 'Engineering', false,
    1770022800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-02', 'roundups', 'github', 'GitHub', false,
    1770022800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-02', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1770022800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-02', 'roundups', 'kubernetes', 'Kubernetes', true,
    1770022800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-02', 'roundups', 'mcp', 'MCP', true,
    1770022800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-02', 'roundups', 'ml', 'ML', true,
    1770022800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-02', 'roundups', 'open', 'Open', false,
    1770022800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-02', 'roundups', 'open source', 'Open Source', true,
    1770022800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-02', 'roundups', 'roundups', 'Roundups', true,
    1770022800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-02', 'roundups', 'sdks', 'SDKs', true,
    1770022800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-02', 'roundups', 'security', 'Security', true,
    1770022800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-02', 'roundups', 'source', 'Source', false,
    1770022800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-02', 'roundups', 'workflows', 'Workflows', false,
    1770022800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-09', 'roundups', '.net', '.NET', true,
    1770627600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-09', 'roundups', 'agent', 'Agent', false,
    1770627600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-09', 'roundups', 'ai', 'AI', true,
    1770627600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-09', 'roundups', 'ai orchestration', 'AI Orchestration', true,
    1770627600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-09', 'roundups', 'azure', 'Azure', true,
    1770627600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-09', 'roundups', 'azure infrastructure', 'Azure Infrastructure', true,
    1770627600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-09', 'roundups', 'ci/cd', 'CI/CD', true,
    1770627600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-09', 'roundups', 'cloud', 'Cloud', false,
    1770627600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-09', 'roundups', 'cloud native development', 'Cloud Native Development', true,
    1770627600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-09', 'roundups', 'code', 'Code', false,
    1770627600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-09', 'roundups', 'copilot', 'Copilot', false,
    1770627600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-09', 'roundups', 'copilot sdk', 'Copilot SDK', true,
    1770627600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-09', 'roundups', 'data', 'Data', false,
    1770627600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-09', 'roundups', 'data engineering', 'Data Engineering', true,
    1770627600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-09', 'roundups', 'development', 'Development', false,
    1770627600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-09', 'roundups', 'devops', 'DevOps', true,
    1770627600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-09', 'roundups', 'engineering', 'Engineering', false,
    1770627600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-09', 'roundups', 'fabric', 'Fabric', false,
    1770627600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-09', 'roundups', 'github', 'GitHub', false,
    1770627600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-09', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1770627600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-09', 'roundups', 'infrastructure', 'Infrastructure', false,
    1770627600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-09', 'roundups', 'kubernetes', 'Kubernetes', true,
    1770627600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-09', 'roundups', 'management', 'Management', false,
    1770627600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-09', 'roundups', 'microsoft', 'Microsoft', false,
    1770627600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-09', 'roundups', 'microsoft fabric', 'Microsoft Fabric', true,
    1770627600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-09', 'roundups', 'ml', 'ML', true,
    1770627600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-09', 'roundups', 'model', 'Model', false,
    1770627600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-09', 'roundups', 'model management', 'Model Management', true,
    1770627600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-09', 'roundups', 'multi', 'Multi', false,
    1770627600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-09', 'roundups', 'multi agent systems', 'Multi Agent Systems', true,
    1770627600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-09', 'roundups', 'native', 'Native', false,
    1770627600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-09', 'roundups', 'orchestration', 'Orchestration', false,
    1770627600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-09', 'roundups', 'roundups', 'Roundups', true,
    1770627600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-09', 'roundups', 'sdk', 'SDK', false,
    1770627600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-09', 'roundups', 'security', 'Security', true,
    1770627600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-09', 'roundups', 'systems', 'Systems', false,
    1770627600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-09', 'roundups', 'vs', 'VS', false,
    1770627600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-09', 'roundups', 'vs code', 'VS Code', true,
    1770627600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-16', 'roundups', '.net', '.NET', true,
    1771232400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-16', 'roundups', 'agents', 'Agents', false,
    1771232400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-16', 'roundups', 'ai', 'AI', true,
    1771232400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-16', 'roundups', 'ai agents', 'AI Agents', true,
    1771232400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-16', 'roundups', 'azure', 'Azure', true,
    1771232400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-16', 'roundups', 'cloud', 'Cloud', false,
    1771232400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-16', 'roundups', 'cloud computing', 'Cloud Computing', true,
    1771232400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-16', 'roundups', 'code', 'Code', false,
    1771232400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-16', 'roundups', 'computing', 'Computing', false,
    1771232400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-16', 'roundups', 'copilot', 'Copilot', false,
    1771232400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-16', 'roundups', 'copilot studio', 'Copilot Studio', true,
    1771232400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-16', 'roundups', 'devops', 'DevOps', true,
    1771232400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-16', 'roundups', 'fabric', 'Fabric', false,
    1771232400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-16', 'roundups', 'fine', 'Fine', false,
    1771232400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-16', 'roundups', 'github', 'GitHub', false,
    1771232400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-16', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1771232400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-16', 'roundups', 'governance', 'Governance', true,
    1771232400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-16', 'roundups', 'jetbrains', 'JetBrains', true,
    1771232400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-16', 'roundups', 'llm', 'LLM', false,
    1771232400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-16', 'roundups', 'llm fine tuning', 'LLM Fine Tuning', true,
    1771232400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-16', 'roundups', 'microsoft', 'Microsoft', false,
    1771232400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-16', 'roundups', 'microsoft fabric', 'Microsoft Fabric', true,
    1771232400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-16', 'roundups', 'ml', 'ML', true,
    1771232400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-16', 'roundups', 'observability', 'Observability', true,
    1771232400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-16', 'roundups', 'openai', 'OpenAI', true,
    1771232400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-16', 'roundups', 'roundups', 'Roundups', true,
    1771232400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-16', 'roundups', 'security', 'Security', true,
    1771232400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-16', 'roundups', 'studio', 'Studio', false,
    1771232400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-16', 'roundups', 'tuning', 'Tuning', false,
    1771232400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-16', 'roundups', 'typescript', 'TypeScript', true,
    1771232400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-16', 'roundups', 'vs', 'VS', true,
    1771232400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-16', 'roundups', 'vs code', 'VS Code', true,
    1771232400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-23', 'roundups', '.net', '.NET', true,
    1771837200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-23', 'roundups', 'agents', 'Agents', false,
    1771837200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-23', 'roundups', 'ai', 'AI', true,
    1771837200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-23', 'roundups', 'ai agents', 'AI Agents', true,
    1771837200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-23', 'roundups', 'ai models', 'AI Models', true,
    1771837200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-23', 'roundups', 'automation', 'Automation', false,
    1771837200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-23', 'roundups', 'azure', 'Azure', true,
    1771837200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-23', 'roundups', 'ci/cd', 'CI/CD', true,
    1771837200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-23', 'roundups', 'cloud', 'Cloud', false,
    1771837200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-23', 'roundups', 'cloud reliability', 'Cloud Reliability', true,
    1771837200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-23', 'roundups', 'code', 'Code', false,
    1771837200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-23', 'roundups', 'copilot', 'Copilot', false,
    1771837200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-23', 'roundups', 'data', 'Data', false,
    1771837200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-23', 'roundups', 'data engineering', 'Data Engineering', true,
    1771837200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-23', 'roundups', 'devops', 'DevOps', true,
    1771837200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-23', 'roundups', 'engineering', 'Engineering', false,
    1771837200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-23', 'roundups', 'enterprise', 'Enterprise', false,
    1771837200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-23', 'roundups', 'enterprise migration', 'Enterprise Migration', true,
    1771837200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-23', 'roundups', 'foundry', 'Foundry', false,
    1771837200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-23', 'roundups', 'github', 'GitHub', false,
    1771837200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-23', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1771837200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-23', 'roundups', 'kubernetes', 'Kubernetes', true,
    1771837200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-23', 'roundups', 'microsoft', 'Microsoft', false,
    1771837200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-23', 'roundups', 'microsoft foundry', 'Microsoft Foundry', true,
    1771837200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-23', 'roundups', 'migration', 'Migration', false,
    1771837200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-23', 'roundups', 'ml', 'ML', true,
    1771837200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-23', 'roundups', 'models', 'Models', false,
    1771837200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-23', 'roundups', 'open', 'Open', false,
    1771837200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-23', 'roundups', 'open source', 'Open Source', true,
    1771837200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-23', 'roundups', 'reliability', 'Reliability', false,
    1771837200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-23', 'roundups', 'roundups', 'Roundups', true,
    1771837200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-23', 'roundups', 'security', 'Security', true,
    1771837200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-23', 'roundups', 'source', 'Source', false,
    1771837200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-23', 'roundups', 'vs', 'VS', false,
    1771837200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-23', 'roundups', 'vs code', 'VS Code', true,
    1771837200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-23', 'roundups', 'workflow', 'Workflow', false,
    1771837200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-02-23', 'roundups', 'workflow automation', 'Workflow Automation', true,
    1771837200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', '(aks)', '(AKS)', false,
    1773651600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', '(model', '(Model', false,
    1773651600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', '.net', '.NET', false,
    1773651600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', '.net 11', '.NET 11', true,
    1773651600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', '11', '11', false,
    1773651600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', 'actions', 'Actions', false,
    1773651600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', 'agent', 'Agent', false,
    1773651600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', 'agentic', 'Agentic', false,
    1773651600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', 'agentic workflows', 'Agentic Workflows', true,
    1773651600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', 'azure', 'Azure', false,
    1773651600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', 'azure kubernetes service (aks)', 'Azure Kubernetes Service (AKS)', true,
    1773651600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', 'azure sre agent', 'Azure SRE Agent', true,
    1773651600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', 'code', 'Code', false,
    1773651600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', 'codeql', 'CodeQL', true,
    1773651600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', 'context', 'Context', false,
    1773651600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', 'copilot', 'Copilot', false,
    1773651600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', 'fabric', 'Fabric', false,
    1773651600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', 'foundry', 'Foundry', false,
    1773651600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', 'framework', 'Framework', false,
    1773651600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', 'github', 'GitHub', false,
    1773651600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', 'github actions', 'GitHub Actions', true,
    1773651600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1773651600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', 'jetbrains', 'JetBrains', true,
    1773651600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', 'kubernetes', 'Kubernetes', false,
    1773651600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', 'mcp', 'MCP', false,
    1773651600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', 'mcp (model context protocol)', 'MCP (Model Context Protocol)', true,
    1773651600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', 'microsoft', 'Microsoft', false,
    1773651600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', 'microsoft agent framework', 'Microsoft Agent Framework', true,
    1773651600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', 'microsoft fabric', 'Microsoft Fabric', true,
    1773651600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', 'microsoft foundry', 'Microsoft Foundry', true,
    1773651600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', 'opentelemetry', 'OpenTelemetry', true,
    1773651600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', 'protocol)', 'Protocol)', false,
    1773651600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', 'scanning', 'Scanning', false,
    1773651600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', 'secret', 'Secret', false,
    1773651600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', 'secret scanning', 'Secret Scanning', true,
    1773651600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', 'service', 'Service', false,
    1773651600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', 'sre', 'SRE', false,
    1773651600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', 'vs', 'VS', false,
    1773651600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', 'vs code', 'VS Code', true,
    1773651600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-16', 'roundups', 'workflows', 'Workflows', false,
    1773651600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', '(mcp)', '(MCP)', false,
    1774256400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', 'actions', 'Actions', false,
    1774256400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', 'advanced', 'Advanced', false,
    1774256400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', 'agent', 'Agent', false,
    1774256400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', 'agent runtime', 'Agent Runtime', true,
    1774256400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', 'agents', 'Agents', false,
    1774256400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', 'ai', 'AI', false,
    1774256400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', 'aks', 'AKS', true,
    1774256400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', 'azure', 'Azure', false,
    1774256400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', 'azure ai foundry', 'Azure AI Foundry', true,
    1774256400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', 'azure devops', 'Azure DevOps', true,
    1774256400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', 'code', 'Code', false,
    1774256400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', 'coding', 'Coding', false,
    1774256400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', 'context', 'Context', false,
    1774256400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', 'copilot', 'Copilot', false,
    1774256400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', 'copilot coding agent', 'Copilot Coding Agent', true,
    1774256400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', 'devops', 'DevOps', false,
    1774256400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', 'enterprise', 'Enterprise', false,
    1774256400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', 'fabric', 'Fabric', false,
    1774256400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', 'foundry', 'Foundry', false,
    1774256400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', 'github', 'GitHub', false,
    1774256400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', 'github actions', 'GitHub Actions', true,
    1774256400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', 'github advanced security', 'GitHub Advanced Security', true,
    1774256400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1774256400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', 'github enterprise server', 'GitHub Enterprise Server', true,
    1774256400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', 'microsoft', 'Microsoft', false,
    1774256400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', 'microsoft fabric', 'Microsoft Fabric', true,
    1774256400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', 'model', 'Model', false,
    1774256400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', 'model context protocol (mcp)', 'Model Context Protocol (MCP)', true,
    1774256400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', 'onelake', 'OneLake', true,
    1774256400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', 'opentelemetry', 'OpenTelemetry', true,
    1774256400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', 'protocol', 'Protocol', false,
    1774256400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', 'runtime', 'Runtime', false,
    1774256400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', 'scanning', 'Scanning', false,
    1774256400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', 'secret', 'Secret', false,
    1774256400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', 'secret scanning', 'Secret Scanning', true,
    1774256400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', 'security', 'Security', false,
    1774256400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', 'server', 'Server', false,
    1774256400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', 'vs', 'VS', false,
    1774256400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-23', 'roundups', 'vs code copilot agents', 'VS Code Copilot Agents', true,
    1774256400, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', '(mcp)', '(MCP)', false,
    1774861200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', 'actions', 'Actions', false,
    1774861200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', 'agents', 'agents', false,
    1774861200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', 'ai', 'AI', false,
    1774861200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', 'ai agents', 'AI agents', true,
    1774861200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', 'aks', 'AKS', true,
    1774861200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', 'azure', 'Azure', false,
    1774861200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', 'azure ai foundry', 'Azure AI Foundry', true,
    1774861200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', 'chain', 'chain', false,
    1774861200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', 'codeql', 'CodeQL', true,
    1774861200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', 'context', 'Context', false,
    1774861200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', 'copilot', 'Copilot', false,
    1774861200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', 'data', 'Data', false,
    1774861200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', 'data factory', 'Data Factory', true,
    1774861200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', 'dataflow', 'Dataflow', false,
    1774861200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', 'dataflow gen2', 'Dataflow Gen2', true,
    1774861200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', 'dbt', 'dbt', true,
    1774861200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', 'fabric', 'Fabric', false,
    1774861200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', 'factory', 'Factory', false,
    1774861200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', 'foundry', 'Foundry', false,
    1774861200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', 'foundry local', 'Foundry Local', true,
    1774861200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', 'gen2', 'Gen2', false,
    1774861200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', 'github', 'GitHub', false,
    1774861200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', 'github actions', 'GitHub Actions', true,
    1774861200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1774861200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', 'link', 'Link', false,
    1774861200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', 'local', 'Local', false,
    1774861200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', 'microsoft', 'Microsoft', false,
    1774861200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', 'microsoft fabric', 'Microsoft Fabric', true,
    1774861200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', 'model', 'Model', false,
    1774861200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', 'model context protocol (mcp)', 'Model Context Protocol (MCP)', true,
    1774861200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', 'private', 'Private', false,
    1774861200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', 'private link', 'Private Link', true,
    1774861200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', 'protocol', 'Protocol', false,
    1774861200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', 'pull', 'Pull', false,
    1774861200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', 'pull requests', 'Pull requests', true,
    1774861200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', 'requests', 'requests', false,
    1774861200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', 'security', 'security', false,
    1774861200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', 'supply', 'Supply', false,
    1774861200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-03-30', 'roundups', 'supply chain security', 'Supply chain security', true,
    1774861200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', '(mcp)', '(MCP)', false,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'actions', 'Actions', false,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'agent', 'agent', false,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'ai', 'AI', false,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'azure', 'Azure', false,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'azure ai foundry', 'Azure AI Foundry', true,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'azure service bus', 'Azure Service Bus', true,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'bus', 'Bus', false,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'chain', 'chain', false,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'cli', 'CLI', false,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'cloud', 'cloud', false,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'code', 'Code', false,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'codeql', 'CodeQL', true,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'context', 'Context', false,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'copilot', 'Copilot', false,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'copilot cli', 'Copilot CLI', true,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'copilot cloud agent', 'Copilot cloud agent', true,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'copilot sdk', 'Copilot SDK', true,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'copilot studio', 'Copilot Studio', true,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'fabric', 'Fabric', false,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'foundry', 'Foundry', false,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'framework', 'Framework', false,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'github', 'GitHub', false,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'github actions', 'GitHub Actions', true,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'microsoft', 'Microsoft', false,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'microsoft agent framework', 'Microsoft Agent Framework', true,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'microsoft fabric', 'Microsoft Fabric', true,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'model', 'Model', false,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'model context protocol (mcp)', 'Model Context Protocol (MCP)', true,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'protocol', 'Protocol', false,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'sdk', 'SDK', false,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'security', 'security', false,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'service', 'Service', false,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'studio', 'Studio', false,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'supply', 'Supply', false,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'supply chain security', 'Supply chain security', true,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'visual', 'Visual', false,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'visual studio', 'Visual Studio', true,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'vs', 'VS', false,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-06', 'roundups', 'vs code', 'VS Code', true,
    1775466000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', '(mcp)', '(MCP)', false,
    1776070800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', 'actions', 'Actions', false,
    1776070800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', 'agent', 'Agent', false,
    1776070800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', 'agents', 'Agents', false,
    1776070800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', 'ai', 'AI', false,
    1776070800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', 'application', 'Application', false,
    1776070800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', 'application insights', 'Application Insights', true,
    1776070800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', 'azure', 'Azure', false,
    1776070800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', 'azure ai foundry', 'Azure AI Foundry', true,
    1776070800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', 'byok', 'BYOK', true,
    1776070800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', 'cli', 'CLI', false,
    1776070800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', 'cloud', 'Cloud', false,
    1776070800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', 'code', 'Code', false,
    1776070800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', 'context', 'Context', false,
    1776070800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', 'copilot', 'Copilot', false,
    1776070800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', 'copilot cli', 'Copilot CLI', true,
    1776070800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', 'copilot cloud agent', 'Copilot Cloud Agent', true,
    1776070800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', 'devsecops', 'DevSecOps', true,
    1776070800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', 'entra', 'Entra', false,
    1776070800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', 'foundry', 'Foundry', false,
    1776070800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', 'foundry local', 'Foundry Local', true,
    1776070800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', 'github', 'GitHub', false,
    1776070800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', 'github actions', 'GitHub Actions', true,
    1776070800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1776070800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', 'id', 'ID', false,
    1776070800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', 'identity', 'Identity', false,
    1776070800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', 'insights', 'Insights', false,
    1776070800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', 'local', 'Local', false,
    1776070800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', 'local models', 'Local Models', true,
    1776070800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', 'managed', 'Managed', false,
    1776070800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', 'managed identity', 'Managed Identity', true,
    1776070800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', 'microsoft', 'Microsoft', false,
    1776070800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', 'microsoft entra id', 'Microsoft Entra ID', true,
    1776070800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', 'model', 'Model', false,
    1776070800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', 'model context protocol (mcp)', 'Model Context Protocol (MCP)', true,
    1776070800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', 'models', 'Models', false,
    1776070800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', 'opentelemetry', 'OpenTelemetry', true,
    1776070800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', 'protocol', 'Protocol', false,
    1776070800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', 'vs', 'VS', false,
    1776070800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-13', 'roundups', 'vs code agents', 'VS Code Agents', true,
    1776070800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', '(aks)', '(AKS)', false,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', '(model', '(Model', false,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', '.net', '.NET', false,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', '.net 11', '.NET 11', true,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', '11', '11', false,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', 'agent', 'Agent', false,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', 'agent governance', 'Agent Governance', true,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', 'api', 'API', false,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', 'azure', 'Azure', false,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', 'azure kubernetes service (aks)', 'Azure Kubernetes Service (AKS)', true,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', 'cli', 'CLI', false,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', 'cloud', 'Cloud', false,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', 'context', 'Context', false,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', 'copilot', 'Copilot', false,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', 'copilot cli', 'Copilot CLI', true,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', 'copilot cloud agent', 'Copilot Cloud Agent', true,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', 'custom', 'Custom', false,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', 'custom skills', 'Custom Skills', true,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', 'data', 'Data', false,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', 'data residency', 'Data Residency', true,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', 'fabric', 'Fabric', false,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', 'fedramp', 'FedRAMP', true,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', 'foundry', 'Foundry', false,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', 'gateway', 'Gateway', false,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', 'github', 'GitHub', false,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', 'governance', 'Governance', false,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', 'kubernetes', 'Kubernetes', false,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', 'kubernetes gateway api', 'Kubernetes Gateway API', true,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', 'mcp', 'MCP', false,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', 'mcp (model context protocol)', 'MCP (Model Context Protocol)', true,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', 'microsoft', 'Microsoft', false,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', 'microsoft fabric', 'Microsoft Fabric', true,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', 'microsoft foundry', 'Microsoft Foundry', true,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', 'model', 'Model', false,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', 'model routing', 'Model Routing', true,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', 'onelake', 'OneLake', true,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', 'protocol)', 'Protocol)', false,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', 'residency', 'Residency', false,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', 'routing', 'Routing', false,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', 'service', 'Service', false,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-20', 'roundups', 'skills', 'Skills', false,
    1776675600, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-27', 'roundups', '5.5', '5.5', false,
    1777273200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-27', 'roundups', 'actions', 'Actions', false,
    1777273200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-27', 'roundups', 'agent', 'Agent', false,
    1777273200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-27', 'roundups', 'agents', 'Agents', false,
    1777273200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-27', 'roundups', 'ai', 'AI', false,
    1777273200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-27', 'roundups', 'azure', 'Azure', false,
    1777273200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-27', 'roundups', 'azure ai foundry', 'Azure AI Foundry', true,
    1777273200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-27', 'roundups', 'azure devops', 'Azure DevOps', true,
    1777273200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-27', 'roundups', 'cli', 'CLI', false,
    1777273200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-27', 'roundups', 'code', 'Code', false,
    1777273200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-27', 'roundups', 'codeql', 'CodeQL', true,
    1777273200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-27', 'roundups', 'copilot', 'Copilot', false,
    1777273200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-27', 'roundups', 'copilot cli', 'Copilot CLI', true,
    1777273200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-27', 'roundups', 'defender', 'Defender', false,
    1777273200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-27', 'roundups', 'devops', 'DevOps', false,
    1777273200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-27', 'roundups', 'fabric', 'Fabric', false,
    1777273200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-27', 'roundups', 'foundry', 'Foundry', false,
    1777273200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-27', 'roundups', 'framework', 'Framework', false,
    1777273200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-27', 'roundups', 'github', 'GitHub', false,
    1777273200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-27', 'roundups', 'github actions', 'GitHub Actions', true,
    1777273200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-27', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1777273200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-27', 'roundups', 'gpt', 'GPT', false,
    1777273200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-27', 'roundups', 'gpt 5.5', 'GPT 5.5', true,
    1777273200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-27', 'roundups', 'identity', 'Identity', false,
    1777273200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-27', 'roundups', 'link', 'Link', false,
    1777273200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-27', 'roundups', 'managed', 'Managed', false,
    1777273200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-27', 'roundups', 'managed identity', 'Managed Identity', true,
    1777273200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-27', 'roundups', 'mcp', 'MCP', true,
    1777273200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-27', 'roundups', 'microsoft', 'Microsoft', false,
    1777273200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-27', 'roundups', 'microsoft agent framework', 'Microsoft Agent Framework', true,
    1777273200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-27', 'roundups', 'microsoft defender xdr', 'Microsoft Defender XDR', true,
    1777273200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-27', 'roundups', 'microsoft fabric', 'Microsoft Fabric', true,
    1777273200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-27', 'roundups', 'mlflow', 'MLflow', true,
    1777273200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-27', 'roundups', 'private', 'Private', false,
    1777273200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-27', 'roundups', 'private link', 'Private Link', true,
    1777273200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-27', 'roundups', 'roundups', 'Roundups', true,
    1777273200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-27', 'roundups', 'vs', 'VS', false,
    1777273200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-27', 'roundups', 'vs code agents', 'VS Code Agents', true,
    1777273200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-04-27', 'roundups', 'xdr', 'XDR', false,
    1777273200, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', '5.5', '5.5', false,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'a2a', 'A2A', false,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'a2a protocol', 'A2A Protocol', true,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'actions', 'Actions', false,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'agent', 'Agent', false,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'agent governance', 'Agent Governance', true,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'agent workflows', 'Agent Workflows', true,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'ai', 'AI', false,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'ai credits', 'AI Credits', true,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'azure', 'Azure', false,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'azure ai foundry', 'Azure AI Foundry', true,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'azure monitor sli/slo', 'Azure Monitor SLI/SLO', true,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'based', 'Based', false,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'billing', 'Billing', false,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'copilot', 'Copilot', false,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'credits', 'Credits', false,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'fabric', 'Fabric', false,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'foundry', 'Foundry', false,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'framework', 'Framework', false,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'github', 'GitHub', false,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'github actions runners', 'GitHub Actions Runners', true,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'governance', 'Governance', false,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'gpt', 'GPT', false,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'gpt 5.5', 'GPT 5.5', true,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'mcp', 'MCP', true,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'microsoft', 'Microsoft', false,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'microsoft agent framework', 'Microsoft Agent Framework', true,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'microsoft fabric onelake', 'Microsoft Fabric OneLake', true,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'microsoft foundry', 'Microsoft Foundry', true,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'monitor', 'Monitor', false,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'onelake', 'OneLake', false,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'protocol', 'Protocol', false,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'roundups', 'Roundups', true,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'runners', 'Runners', false,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'sli/slo', 'SLI/SLO', false,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'token', 'Token', false,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'token billing', 'Token Billing', true,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'usage', 'Usage', false,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'usage based billing', 'Usage Based Billing', true,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-04', 'roundups', 'workflows', 'Workflows', false,
    1777878000, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', '.net', '.NET', true,
    1778482800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', 'agent', 'Agent', false,
    1778482800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', 'ai', 'AI', false,
    1778482800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', 'azure', 'Azure', false,
    1778482800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', 'azure ai foundry', 'Azure AI Foundry', true,
    1778482800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', 'azure virtual machines', 'Azure Virtual Machines', true,
    1778482800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', 'byok', 'BYOK', true,
    1778482800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', 'cli', 'CLI', false,
    1778482800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', 'code', 'Code', false,
    1778482800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', 'copilot', 'Copilot', false,
    1778482800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', 'copilot cli', 'Copilot CLI', true,
    1778482800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', 'databricks', 'Databricks', true,
    1778482800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', 'defender', 'Defender', false,
    1778482800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', 'devops', 'DevOps', true,
    1778482800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', 'enterprise', 'Enterprise', false,
    1778482800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', 'fabric', 'Fabric', false,
    1778482800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', 'foundry', 'Foundry', false,
    1778482800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', 'framework', 'Framework', false,
    1778482800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', 'github', 'GitHub', false,
    1778482800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', 'github copilot', 'GitHub Copilot', true,
    1778482800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', 'github enterprise', 'GitHub Enterprise', true,
    1778482800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', 'governance', 'Governance', false,
    1778482800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', 'machines', 'Machines', false,
    1778482800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', 'mcp', 'MCP', true,
    1778482800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', 'microsoft', 'Microsoft', false,
    1778482800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', 'microsoft agent framework', 'Microsoft Agent Framework', true,
    1778482800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', 'microsoft defender', 'Microsoft Defender', true,
    1778482800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', 'microsoft fabric', 'Microsoft Fabric', true,
    1778482800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', 'ml', 'ML', true,
    1778482800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', 'model', 'Model', false,
    1778482800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', 'model governance', 'Model Governance', true,
    1778482800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', 'opentelemetry', 'OpenTelemetry', true,
    1778482800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', 'repository', 'Repository', false,
    1778482800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', 'repository rulesets', 'Repository Rulesets', true,
    1778482800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', 'roundups', 'Roundups', true,
    1778482800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', 'rulesets', 'Rulesets', false,
    1778482800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', 'security', 'Security', true,
    1778482800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', 'virtual', 'Virtual', false,
    1778482800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', 'vs', 'VS', false,
    1778482800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;
INSERT INTO content_tags_expanded (
    slug, collection_name, tag_word, tag_display, is_full_tag,
    date_epoch, is_ai, is_azure, is_dotnet, is_devops,
    is_github_copilot, is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-dotnet-roundup-2026-05-11', 'roundups', 'vs code', 'VS Code', true,
    1778482800, false, false, true, false,
    false, false, false, 4
) ON CONFLICT DO NOTHING;


-- ── processed_urls ─────────────────────────────────────────────────────

INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2025-07-07', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2025-07-07', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2025-07-14', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2025-07-14', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2025-07-21', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2025-07-21', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2025-07-28', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2025-07-28', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2025-08-04', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2025-08-04', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2025-08-11', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2025-08-11', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2025-08-18', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2025-08-18', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2025-08-25', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2025-08-25', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2025-09-01', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2025-09-01', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2025-09-08', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2025-09-08', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2025-09-15', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2025-09-15', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2025-09-22', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2025-09-22', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2025-09-29', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2025-09-29', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2025-10-06', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2025-10-06', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2025-10-13', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2025-10-13', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2025-10-20', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2025-10-20', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2025-10-27', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2025-10-27', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2025-11-03', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2025-11-03', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2025-11-10', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2025-11-10', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2025-11-17', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2025-11-17', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2025-11-24', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2025-11-24', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2025-12-01', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2025-12-01', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2025-12-08', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2025-12-08', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2025-12-15', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2025-12-15', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2025-12-22', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2025-12-22', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2025-12-29', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2025-12-29', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2026-01-05', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2026-01-05', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2026-01-12', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2026-01-12', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2026-01-19', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2026-01-19', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2026-01-26', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2026-01-26', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2026-02-02', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2026-02-02', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2026-02-09', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2026-02-09', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2026-02-16', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2026-02-16', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2026-02-23', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2026-02-23', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2026-03-16', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2026-03-16', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2026-03-23', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2026-03-23', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2026-03-30', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2026-03-30', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2026-04-06', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2026-04-06', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2026-04-13', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2026-04-13', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2026-04-20', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2026-04-20', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2026-04-27', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2026-04-27', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2026-05-04', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2026-05-04', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();
INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug, reason)
VALUES ('/dotnet/roundups/weekly-dotnet-roundup-2026-05-11', 'succeeded', 'TechHub', 'roundups', 'weekly-dotnet-roundup-2026-05-11', 'roundup-generated')
ON CONFLICT (external_url) DO UPDATE SET
    slug            = EXCLUDED.slug,
    collection_name = EXCLUDED.collection_name,
    status          = 'succeeded',
    updated_at      = NOW();

-- weekly-dotnet-roundup-2026-03-02
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2026-03-02', 'roundups', 'Weekly Overview: AI Automation, Cloud Engineering Updates, and Secure DevOps Practices',
    'The Coding section offers guides for in-depth debugging and instrumentation, helping teams diagnose container builds and monitor .NET workloads with minimal overhead.

<!--excerpt_end-->

### Debugging Dockerfiles in Visual Studio Code Using the Debug Adapter Protocol

A new tutorial shows how VS Code leverages the Debug Adapter Protocol (DAP) for interactive Dockerfile debugging. Features include setting breakpoints, stepping through build stages, and inspecting commands during image creation. Integration with VS Code extensions and Docker tooling improves the troubleshooting process. Supporting resources are available for hands-on exploration.

- [Debugging Dockerfiles in Visual Studio Code Using the Debug Adapter Protocol](/dotnet/videos/debugging-dockerfiles-in-visual-studio-code-using-the-debug-adapter-protocol)

### Recording Metrics In-Process with MeterListener Using System.Diagnostics.Metrics

Andrew Lock details the use of MeterListener in .NET for collecting in-process metrics. Examples include a simple ASP.NET Core app with interactive visualization using Spectre.Console and a custom MetricManager for filtering and tracking resource metrics like routing, memory, and CPU. Scheduling and aggregation are handled via BackgroundService and concurrency with Interlocked. The approach supports prototyping and forms a foundation for advanced app-level monitoring.

- [Recording Metrics In-Process with MeterListener Using System.Diagnostics.Metrics](https://andrewlock.net/recording-metrics-in-process-using-meterlistener/)',
    'The Coding section offers guides for in-depth debugging and instrumentation, helping teams diagnose container builds and monitor .NET workloads with minimal overhead.',
    1772442000, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2026-03-02', 'TechHub',
    'TechHub', '87df5e9c5bb78d86e1f4c5776c2edf40', ',AI,Azure,.NET,DevOps,GitHub Copilot,ML,Security,Roundups,',
    false, false, true, false, false,
    false, false, 4, NULL
) ON CONFLICT (collection_name, slug) DO UPDATE SET
    title      = EXCLUDED.title,
    content    = EXCLUDED.content,
    excerpt    = EXCLUDED.excerpt,
    updated_at = NOW();
-- weekly-dotnet-roundup-2026-03-09
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask, ai_metadata
) VALUES (
    'weekly-dotnet-roundup-2026-03-09', 'roundups', 'GitHub Copilot Updates, Durable AI Agents, and Secure Cloud Features',
    'In Coding this week, major platforms received new updates and improvements. TypeScript 6.0 RC debuted, VS Code expanded support for agent development, and .NET streamlined data and workflow integration. Tutorials and Q&A sessions cover current best practices in productivity and migration.

<!--excerpt_end-->

### TypeScript 6.0 Release Candidate and Migration Path

TypeScript 6.0 RC introduces default strictness settings, native support for Temporal date types, and more flexible module resolution. The `--stableTypeOrdering` flag supports the coming parallel type checker. Updates to standard libraries and the removal of deprecated configuration targets modernize projects. Codemod tools help with migration, and developers are encouraged to check compatibility ahead of TypeScript 7.0, which will feature a Go language implementation for the compiler and tools.

- [Announcing TypeScript 6.0 Release Candidate: Features and Deprecations](https://devblogs.microsoft.com/typescript/announcing-typescript-6-0-rc/)

### Visual Studio Code 1.110: Agent-Driven Development and Insiders Update

VS Code 1.110 brings more agent-specific features, including session memory retention, manual compaction with `/compact`, request queuing, session forking, and lifecycle hooks for workflow automation. Manual and automatic session features support different workflows. Frontend tools, CLI integration, and live session updates round out the release.

Insiders version improvements focus on accessibility, dialogs, keyboard navigation, and terminal experience.

- [Making Agents Practical for Real-World Development with VS Code 1.110](https://code.visualstudio.com/blogs/2026/03/05/making-agents-practical-for-real-world-development)
- [February 2026 Insiders (version 1.110): What’s New in Visual Studio Code](https://code.visualstudio.com/updates/v1_110)

### .NET Data and Workflow Tooling

The .NET Data Standup presents dotConnect and Entity Developer tools for streamlined data access and model automation. This reduces manual configuration and helps avoid errors, supporting quick setup and maintenance.

Community guides offer insight into orchestrating advanced workflows in the Microsoft Agent Framework, encouraging modular and graph-based automation.

- [.NET Data Community Standup: How dotConnect and Entity Developer Streamline .NET Workflows](/dotnet/videos/net-data-community-standup-how-dotconnect-and-entity-developer-streamline-net-workflows)
- [Demystifying Custom Orchestration in Microsoft Agent Framework Workflows](/dotnet/videos/demystifying-custom-orchestration-in-microsoft-agent-framework-workflows)

### .NET MAUI Platform Expansion and Blazor in .NET 11 Preview

This week’s .NET MAUI standup covers expanded platform support (including Linux and macOS), plus AI-driven acceleration for routine app development. Blazor Community Standup discusses productivity, performance, and tooling improvements in .NET 11 Preview 2 for web development.

- [.NET MAUI Community Standup: Expanding to New Platforms and Accelerating with AI](/ai/videos/net-maui-community-standup-expanding-to-new-platforms-and-accelerating-with-ai)
- [Blazor Community Standup: What’s New in ASP.NET Core & Blazor for .NET 11 Preview 2](/dotnet/videos/blazor-community-standup-whats-new-in-aspnet-core-and-blazor-for-net-11-preview-2)',
    'In Coding this week, major platforms received new updates and improvements. TypeScript 6.0 RC debuted, VS Code expanded support for agent development, and .NET streamlined data and workflow integration. Tutorials and Q&A sessions cover current best practices in productivity and migration.',
    1773046800, 'dotnet', '/dotnet/roundups/weekly-dotnet-roundup-2026-03-09', 'TechHub',
    'TechHub', 'd503a25727946ee3d32738784e03e794', ',AI,Azure,.NET,DevOps,GitHub Copilot,ML,Security,Roundups,',
    false, false, true, false, false,
    false, false, 4, NULL
) ON CONFLICT (collection_name, slug) DO UPDATE SET
    title      = EXCLUDED.title,
    content    = EXCLUDED.content,
    excerpt    = EXCLUDED.excerpt,
    updated_at = NOW();
