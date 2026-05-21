-- Migration 033: Roundup INSERTs for section 'ml'
-- Generated: 2026-05-21 from localhost database (AI-regenerated metadata).
-- Safe to re-run: ON CONFLICT DO UPDATE overwrites all fields with source-of-truth values.

-- ── content_items ─────────────────────────────────────────────────────────────────
-- weekly-ml-roundup-2026-05-11
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ml-roundup-2026-05-11', 'roundups', 'Weekly Machine Learning Roundup: Ops guardrails and context control',
    'This week in machine learning and analytics tooling was mostly about making day-to-day platform operations less fragile: Fabric pushed several previews that help teams scale Spark automation, find assets across workspaces, and centralize monitoring and cost controls, while Databricks guidance focused on disaster recovery and visibility across sprawling workspaces. Building on last week''s Fabric-heavy focus on "operational plumbing" (MLOps boundaries with MLflow, real-time ingestion paths, and secure-by-default architecture choices), the throughline here is similar: once the platform grows beyond a single workspace or a single team, automation, discoverability, and guardrails matter as much as the model code. Alongside the platform work, model-behavior guidance reinforced a practical theme: better outcomes come from better context, not just bigger prompts.
<!--excerpt_end-->
## Microsoft Fabric: Scaling Spark automation, discoverability, and operational guardrails
Fabric''s Spark automation story moved forward with preview support for High Concurrency sessions in the Fabric Livy API, aimed at teams running many parallel jobs without paying the overhead of constantly spinning up new sessions. Instead of serializing work through a single session, you can run multiple isolated Spark workloads in parallel while still reusing sessions, tagging them with `sessionTag` so clients can reliably reconnect to the right session and track what is running where. In practice, this changes how you build orchestration: rather than treating a Livy session as a single-threaded bottleneck, you can design job runners that multiplex workloads, isolate failure domains, and get cleaner monitoring and cost attribution because sessions are no longer a black box shared by unrelated jobs. If last week''s theme was promoting ML work safely across environments (Dev/Test/Prod boundaries with MLflow), this is the adjacent scaling problem: once you have repeatable pipelines, you need them to run concurrently without turning into a session-management and cost-debugging mess.
Cross-workspace sprawl is another common pain point in Fabric, and a new preview OneLake Catalog Search REST API targets exactly that. It lets you discover items across workspaces programmatically, which matters once you have dozens of domains, duplicated datasets, and a growing list of notebooks, warehouses, and semantic models. This ties directly back to last week''s cross-workspace MLOps story: separating workspaces is only helpful if teams can still find the right datasets, artifacts, and owners across those boundaries without manual inventories. Microsoft also wired this search into the Fabric Core MCP Server (so agent-driven workflows can query the catalog as a tool) and added a new `fab find` command in the Fabric CLI. If you are building internal tooling or CI checks, the details here are practical: results can be filtered and shaped using JMESPath, making it easier to build repeatable "what do we have and where is it" scripts without maintaining your own inventory tables.
Operationally, Fabric also added a preview feature in the monitoring hub that centralizes failure notification management for scheduled items. The new Schedule failures page consolidates the configuration and maintenance of email failure notifications across workspaces, reducing the common drift where some pipelines notify the right owners and others silently fail. This is not a new scheduler, but it is a step toward treating alerting as a platform setting instead of a per-item afterthought. That lands neatly after last week''s architecture guidance on designing maintainable lakehouse pipelines (idempotency, retries, observability): you can make the pipeline logic robust, but you still need consistent operational ownership when something breaks at 2am.
On the cost and capacity side, Fabric moved a set of tools to general availability: updates to the capacity metrics app now include a Capacity health page plus timepoint summary and timepoint detail views, which are designed to make throttling and capacity pressure easier to spot and explain at a specific point in time. The Fabric Chargeback app is also now generally available, giving teams a supported path to allocate capacity costs across workspaces and workloads, which pairs naturally with the push toward more parallel Spark usage and better monitoring. In other words, as the platform encourages more automation and concurrency, it is also giving admins and platform teams better levers to answer the inevitable follow-up questions: "what caused the spike?" and "who pays for it?"
Finally, Fabric Data Warehouse got a schema-evolution quality-of-life improvement in preview: T-SQL `ALTER TABLE ... ALTER COLUMN` support for metadata-only schema changes. For many teams, schema evolution is where pipelines become brittle because type changes trigger rebuilds or force complicated migration playbooks. The pitch here is fewer disruptive rebuilds and fewer downstream breaks when the change can be handled as a metadata update, aligning with Delta Lake patterns like type widening. That also echoes last week''s medallion framework guidance on schema evolution and rerun safety: you can design layers well, but you still need the warehouse and lakehouse tooling to make everyday changes survivable for downstream transformations and ML feature tables.
- [High Concurrency Support for the Fabric Livy API— Scalable Spark Automation (Preview)](https://community.fabric.microsoft.com/t5/Fabric-Updates-Blogs/High-Concurrency-Support-for-the-Fabric-Livy-API-Scalable-Spark/ba-p/5178390)
- [Discover items across workspaces with the OneLake Catalog Search API, MCP and CLI tools (Preview)](https://community.fabric.microsoft.com/t5/Fabric-Updates-Blogs/Discover-items-across-workspaces-with-the-OneLake-Catalog-Search/ba-p/5176768)
- [Manage failure notifications from the monitoring hub in Fabric (Preview)](https://community.fabric.microsoft.com/t5/Fabric-Updates-Blogs/Manage-failure-notifications-from-the-monitoring-hub-in-Fabric/ba-p/5160381)
- [Providing more insights & tools: Capacity health, timepoint summary, timepoint detail, chargeback now generally available](https://community.fabric.microsoft.com/t5/Fabric-Updates-Blogs/Providing-more-insights-amp-tools-Capacity-health-timepoint/ba-p/5176753)
- [Simplify Schema Changes in Fabric Data Warehouse with ALTER COLUMN (Preview)](https://community.fabric.microsoft.com/t5/Fabric-Updates-Blogs/Simplify-Schema-Changes-in-Fabric-Data-Warehouse-with-ALTER/ba-p/5177593)
## Microsoft Fabric Dataflow Gen2 and Direct Lake: Less rework in data prep, fewer surprises in semantic performance
Dataflow Gen2 picked up a preview feature that targets a very specific kind of waste: repeated Power Query work copied across pipelines. "My queries" lets you save Power Query (M) queries into a personal library, then import them into other dataflows when you need the same cleaning logic again. That changes the default from copy-paste reuse (which quietly forks logic) to an explicit reuse path, which should help teams standardize transformations like date handling, normalization, and data quality fixes without maintaining separate "template" PBIX files or shared snippets in wikis. It fits with last week''s Fabric data engineering push (nested folder-aware lake transformations and dbt orchestration): the common goal is to make the "last mile" of dataset shaping more maintainable, whether that logic lives in lake transformations, dbt models, or Power Query steps.
On the consumption side, guidance for Direct Lake on SQL with Fabric Data Warehouse focused on what drives real performance when semantic models page Delta Parquet data into memory. The practical takeaway is that schema design and cardinality directly affect whether Direct Lake behaves like you expect, and the article calls out how and why models fall back to DirectQuery. That matters because fallback often shows up as "it was fast yesterday, why is it slow today" once a column''s cardinality grows or a model change increases memory pressure. The best practices here center on designing for VertiPaq-friendly shapes, watching the common causes of fallback, and using those signals to decide whether to remodel, reduce cardinality, or accept DirectQuery for specific queries. Read next to last week''s medallion decision guide and real-time ingestion pattern (SQL change events into Fabric), this is a useful reminder that upstream choices (schema evolution, incremental feeds, partitioning) quickly turn into downstream semantic performance issues if teams do not manage cardinality and model shape intentionally.
- [From repetition to reuse: accelerate data prep with My queries in Dataflow Gen2](https://community.fabric.microsoft.com/t5/Fabric-Updates-Blogs/From-repetition-to-reuse-accelerate-data-prep-with-My-queries-in/ba-p/5176763)
- [Direct Lake on SQL with Fabric Data Warehouse](https://community.fabric.microsoft.com/t5/Fabric-Updates-Blogs/Direct-Lake-on-SQL-with-Fabric-Data-Warehouse/ba-p/5177641)
## Azure Databricks: Disaster recovery planning and a workspace inventory you can query
Two Databricks posts landed on the operational end of machine learning platforms: keep the platform recoverable, and make it observable. A disaster recovery strategy write-up laid out a phased, customer-managed approach that forces the usual hard decisions into the open: what RTO/RPO targets are realistic, when active-active is worth the complexity, and when warm standby is the better trade. It also gets concrete about what must be replicated across regions, including Unity Catalog metadata and Delta data, and it frames DR as something you automate with infrastructure as code (IaC) and repeatable pipelines rather than a runbook you hope to never use. The inclusion of patterns like Delta Sharing and Deep Clone highlights a key detail: data and governance metadata need different replication tactics, and your DR plan needs to cover both. That parallels last week''s Fabric guidance on planning secure streaming paths and maintainable lakehouse layers early: if you treat networking, governance metadata, and data layout as afterthoughts, they become the hardest things to retrofit when you need higher assurance (whether for private connectivity or for cross-region recovery).
The "single pane of glass" post tackled a different, but related, problem: once a Databricks workspace grows, it gets hard to answer basic questions about what exists, who owns it, what it costs, and how it is used. The proposed Discovery utility scans workspace assets (clusters, jobs, warehouses, Delta Live Tables, Unity Catalog objects, security configuration, billing, and utilization), writes the results into Unity Catalog Delta tables, and surfaces them through a Lakeview dashboard. That approach matters because it turns operational visibility into queryable data: you can audit configurations, track utilization patterns, and build internal controls without relying on screenshots or one-off admin scripts. In spirit, it is solving a similar problem to Fabric''s new cross-workspace catalog search and centralized monitoring: at scale, governance and operations start with "can we reliably inventory what we run?"
- [Resilient by Design: Azure Databricks Disaster Recovery Strategy](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/resilient-by-design-azure-databricks-disaster-recovery-strategy/ba-p/4516464)
- [From Chaos to Clarity: Your Databricks Workspace on a Single Pane of Glass](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/from-chaos-to-clarity-your-databricks-workspace-on-a-single-pane/ba-p/4516805)
## Model behavior control: Context engineering, RAG, and when to fine-tune
A model-behavior primer this week pulled together the toolbox teams are actually using in production to make models respond the way they need. It starts with prompt-level controls (zero-shot, one-shot, few-shot examples, and system prompts) and then moves into context engineering, where you shape what the model sees and how it sees it, often by structuring inputs rather than just adding more text. Retrieval-augmented generation (RAG) and embeddings show up here as the practical bridge between static model behavior and dynamic, domain-specific answers, especially when you need responses grounded in internal documents.
When prompts and RAG are not enough, the guidance shifts to model adaptation: fine-tuning and LoRA (low-rank adaptation). The useful framing is that fine-tuning is not the first tool you reach for, but it can be the right one when you need consistent style, structured outputs, or domain behaviors that are hard to achieve through context alone. LoRA is called out as a lighter-weight alternative for adaptation, which is often relevant when you want targeted behavior changes without the cost and operational overhead of full fine-tunes. Read alongside last week''s Fabric MLOps thread (MLflow tracking across workspaces and secure, repeatable promotion), the "behavior control" takeaway is that whichever lever you choose (RAG, fine-tune, LoRA), you still need the operational backbone to version inputs, track experiments, and promote changes safely, otherwise improvements in model behavior are hard to reproduce and govern.
- [How to change model behavior! Context engineering, fine-tuning and more](https://www.youtube.com/watch?v=oqbKWwUhh4Y)',
    'This week in machine learning and analytics tooling was mostly about making day-to-day platform operations less fragile: Fabric pushed several previews that help teams scale Spark automation, find assets across workspaces, and centralize monitoring and cost controls, while Databricks guidance focused on disaster recovery and visibility across sprawling workspaces. Building on last week''s Fabric-heavy focus on "operational plumbing" (MLOps boundaries with MLflow, real-time ingestion paths, and secure-by-default architecture choices), the throughline here is similar: once the platform grows beyond a single workspace or a single team, automation, discoverability, and guardrails matter as much as the model code. Alongside the platform work, model-behavior guidance reinforced a practical theme: better outcomes come from better context, not just bigger prompts.',
    1778482800, 'ml', '/ml/roundups/weekly-ml-roundup-2026-05-11', 'TechHub',
    'TechHub', 'F98198F90992B5C0684EEC3B6B5285F239E70651B78017A9E6CDA3E800036DE2', ',Microsoft Fabric,Apache Spark,Livy API,High Concurrency,OneLake,Catalog Search API,Fabric CLI,JMESPath,Monitoring Hub,Capacity Metrics,Chargeback,Fabric Data Warehouse,Direct Lake,Azure Databricks,Disaster Recovery,Unity Catalog,Delta Lake,IaC,Retrieval Augmented Generation,Embeddings,Fine Tuning,LoRA,Context Engineering,ML,Roundups,',
    false, false, false, false, false,
    true, false, 32
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
-- weekly-ml-roundup-2026-05-04
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ml-roundup-2026-05-04', 'roundups', 'Weekly Machine Learning Roundup: OneLake, streaming, and ML ops',
    'This week, the Machine Learning story was mostly about getting data into shape for ML and analytics at scale: Microsoft Fabric leaned further into OneLake as the common data layer, tightened up real-time streaming so features and signals can arrive with fewer surprises, and nudged SQL developers toward a more modern, Git-friendly workflow in VS Code. Alongside those platform updates, Microsoft also shared an early look at how unconventional hardware (and its digital twins) might run real lending models in the future.
<!--excerpt_end-->
## Microsoft Fabric and OneLake: broader access to governed data and metadata
Fabric expanded the practical ways teams can discover and reuse data without copying it around. A new preview feature, "Mirrored Dremio catalog", mirrors Dremio-managed Apache Iceberg catalog metadata into OneLake using shortcuts, which keeps access effectively zero-copy while still making those tables show up across Fabric workloads. The key idea is that Fabric can "see" the Iceberg tables through the catalog mirror rather than forcing another ingestion path, which is useful if Dremio already owns table layout, optimization, and governance.
On the discovery side, Fabric also introduced a preview OneLake Catalog Search REST API for finding items across workspaces by metadata, with the same capability wired into the Fabric core MCP server and exposed in Fabric CLI as `fab find`. For teams trying to scale ML across multiple domains and workspaces, the value is less time hunting for the right lakehouse/warehouse/semantic model, and more consistent ways to script discovery into tooling (including agentic workflows) using API calls and CLI filtering (including JMESPath).
- [Bring your Dremio data into OneLake (Preview)](https://blog.fabric.microsoft.com/en-US/blog/bring-your-dremio-data-into-onelake-preview/)
- [Discover Fabric items across workspaces with the OneLake Catalog Search API, MCP and CLI tools (Preview)](https://blog.fabric.microsoft.com/en-US/blog/discover-fabric-items-across-workspaces-with-the-onelake-catalog-search-api-mcp-and-cli-tools-preview/)
## Real-time pipelines in Fabric: SQL-based streaming plus better observability
Fabric''s real-time tooling got a clearer "build it, test it, run it, monitor it" arc this week. The Eventstreams SQL operator reached general availability, positioning SQL as a first-class way to express streaming transforms while adding production-ready capabilities like multi-destination fan-out, built-in testing, and event-time processing. Event-time processing matters when late or out-of-order events are normal (common in IoT, clickstreams, and operational logs) because it lets you reason about "when it happened" instead of "when it arrived", which can stabilize aggregations and windowed calculations that downstream ML features depend on.
At the same time, Eventstreams gained workspace monitoring in preview. Enabling it creates a managed monitoring Eventhouse and emits KQL tables that track node status, per-minute throughput, and per-minute error metrics. The guidance to republish existing Eventstreams to pick up monitoring is a practical detail for teams already running production pipelines: instrumentation is becoming part of the product surface, not a separate DIY logging project.
- [Simplify Real-Time Pipelines with the Fabric Eventstreams SQL Operator (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/simplify-real-time-pipelines-with-the-fabric-eventstreams-sql-operator-generally-available/)
- [Monitor your Eventstreams with workspace monitoring (Preview)](https://blog.fabric.microsoft.com/en-US/blog/monitor-your-eventstreams-with-workspace-monitoring-preview/)
## Fabric April 2026 update: MLflow logging, notebooks, and warehouse features that affect ML workflows
The April 2026 Fabric feature summary tied together several changes that land directly in day-to-day ML and analytics work. Fabric added VS Code-based workspace and environment management, which fits the broader theme of moving operational tasks closer to developer tooling. Notebooks picked up retry policies, a small-sounding change that can make scheduled training and feature engineering runs more resilient when transient failures happen.
On the ML lifecycle side, Fabric now supports MLflow cross-workspace logging (including OAP workspaces), which is useful when teams separate experimentation, shared model registries, and production workspaces for governance. Semantic Link (SemPy) advanced to 0.14.0 with admin APIs, which matters for teams automating semantic model management and connecting Power BI semantics to Python-driven analysis and ML feature work.
Data Warehouse improvements like transactional `ALTER TABLE` and `COPY INTO` support for JSONL also feed into ML pipelines, especially when teams stage semi-structured data and want predictable schema evolution and repeatable loads. Real-Time Intelligence updates (including Eventstream observability and an Eventhouse remote MCP) reinforce the push to make streaming systems easier to operate and easier to connect into automated workflows.
- [Fabric April 2026 Feature Summary](https://blog.fabric.microsoft.com/en-US/blog/fabric-april-2026-feature-summary/)
## SQL development for Fabric: Azure Data Studio retirement and the move to VS Code
Fabric SQL developers got a clear direction: Azure Data Studio is retired, and the recommended path is VS Code with SQL Database Projects and the MSSQL extension. The emphasis is on adopting software-engineering workflows for database changes: Git-based source control, pull request reviews, schema compare, and publish script previews so teams can see what a deployment will do before it runs. For ML teams that manage feature-store-like tables or training data schemas in Fabric warehouses, this shift reduces "drift by manual edits" and makes schema changes auditable and reviewable.
The VS Code MSSQL extension''s support for GitHub Copilot is positioned as a productivity boost inside the editor, and Microsoft also called out an ADS migration toolkit to help teams move existing setups rather than starting from scratch.
- [Azure Data Studio to VS Code: What it means for SQL database in Fabric developers](https://blog.fabric.microsoft.com/en-US/blog/azure-data-studio-to-vs-code-what-it-means-for-sql-database-in-fabric-developers/)
## Other Machine Learning News
Fabric pipelines continued to shift from classic ETL toward broader workflow orchestration, with a preview Approval activity that enables human-in-the-loop steps (useful for governance gates like model sign-off, data access approval, or controlled production promotion), plus more focus on observability for long-running workflows.
- [Pipelines are evolving beyond ETL](https://blog.fabric.microsoft.com/en-US/blog/pipelines-are-evolving-beyond-etl/)
A longer-horizon case study looked at a real fintech lending decisioning workload (weighted ensembles with explainability and auditability requirements) evaluated using Microsoft Analog Optical Computer (AOC) digital twins on Azure, offering an early signal of how alternative compute approaches might be tested against regulated ML scenarios before hardware is broadly available.
- [First real-world Lending ML workload evaluated on Microsoft Optical AOC Computer digital twins](https://techcommunity.microsoft.com/t5/azure/first-real-world-lending-ml-workload-evaluated-on-microsoft/m-p/4514914#M22506)',
    'This week, the Machine Learning story was mostly about getting data into shape for ML and analytics at scale: Microsoft Fabric leaned further into OneLake as the common data layer, tightened up real-time streaming so features and signals can arrive with fewer surprises, and nudged SQL developers toward a more modern, Git-friendly workflow in VS Code. Alongside those platform updates, Microsoft also shared an early look at how unconventional hardware (and its digital twins) might run real lending models in the future.',
    1777878000, 'ml', '/ml/roundups/weekly-ml-roundup-2026-05-04', 'TechHub',
    'TechHub', '3D856239CA0ED701E42B7D33FBE0BE7E760615AC1809A1B0A4DAE037DB7D9223', ',Microsoft Fabric,OneLake,Apache Iceberg,Dremio,Data Governance,Data Catalog,REST API,Fabric CLI,JMESPath,Real Time Streaming,Eventstreams,Event Time Processing,KQL,MLflow,VS Code,ML,Roundups,',
    false, false, false, false, false,
    true, false, 32
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
-- weekly-ml-roundup-2026-04-27
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ml-roundup-2026-04-27', 'roundups', 'Weekly Machine Learning Roundup: Fabric MLOps and Secure Streaming',
    'This week in machine learning, the center of gravity was Fabric: Microsoft kept pushing the practical plumbing that turns models into something teams can run repeatedly and safely. The updates focused on tightening the MLOps loop (promoting experiments and models across environments), feeding ML and analytics with fresher data (streaming change events into Fabric), and making data prep more maintainable (better lake folder handling and more orchestration options), with a consistent thread of "do it securely over private networking."
<!--excerpt_end-->
## Microsoft Fabric MLOps with MLflow (cross-workspace logging GA)
Fabric''s ML story got more operational with cross-workspace logging for MLflow now generally available, which directly targets a common MLOps pain point: keeping Dev/Test/Prod separated without breaking your standard experiment tracking and model registry workflows. With this GA release, teams can use the normal MLflow APIs (through `synapseml-mlflow`) to log experiments in one Fabric workspace while registering models in another, so you can keep experimentation noisy and iterative in a Dev workspace but promote approved models into a controlled Prod workspace without hacks or manual exports.
The practical implication is that environment boundaries stop being an obstacle to MLflow-based pipelines. You can keep consistent run history, artifacts, and model lineage while still aligning with workspace-level governance, permissions, and operational practices. The post also calls out network and security considerations that show up immediately in real deployments, including how to think about Outbound Access Protection (OAP) and using managed private endpoints when your MLflow interactions or artifact access need to stay on private paths rather than open outbound routes.
- [Cross-workspace logging for MLflow in Microsoft Fabric: Build MLOps workflows with confidence (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/cross-workspace-logging-for-mlflow-in-microsoft-fabric-build-mlops-workflows-with-confidence-generally-available/)
## Real-time data for ML and analytics in Fabric (SQL change events -> Eventstream)
Fabric''s real-time pipeline story got a concrete blueprint this week with a walkthrough of streaming SQL change events into Microsoft Fabric Eventstream, aimed at teams that want lower-latency features, monitoring signals, or near-real-time analytics without bolting together a custom ingestion stack. The approach uses SQL Server 2025 / Azure SQL Change Event Streaming (CES) to emit database changes as CloudEvents (a standardized event envelope), then delivers them over AMQP or Kafka into Eventstream via an Event Hubs-compatible custom endpoint.
Once the change feed is in Fabric, you can apply real-time transformations and route the data into downstream Fabric destinations suited for fast query and detection workflows, including Eventhouse where KQL becomes the natural way to explore and operationalize those events. For ML use cases, this pattern matters because it reduces the time between "data changed" and "feature/metric updated," which is often the difference between offline reporting and systems that can detect drift, trigger retraining, or drive responsive user experiences.
- [Stream SQL Change Events to Microsoft Fabric Eventstream with Change Event Streaming for Real-Time Analytics](https://blog.fabric.microsoft.com/en-US/blog/stream-sql-change-events-to-microsoft-fabric-eventstream-with-change-event-streaming-for-real-time-analytics/)
## Fabric data engineering improvements for ML readiness (OneLake shortcuts GA, dbt in pipelines preview)
On the data prep side, Fabric added two capabilities that make ML datasets easier to build and keep current when your lake layout and transformations get more complex. First, OneLake shortcut transformations now support nested folders (GA), which sounds small until you run into real partitioned lake structures. With nested folder support, transformations can process recursively across subfolders, detect incremental changes, and write output while preserving the directory structure, all while converting common file formats (CSV/Parquet/JSON) into Delta tables. That combination matters for ML pipelines because it reduces brittle "enumerate folders yourself" logic and makes it easier to keep curated Delta datasets aligned with how data actually lands in the lake.
Second, Fabric pipelines introduced a dbt job activity in Preview, bringing dbt orchestration into the same place teams already schedule ingestion, training data refreshes, and downstream tasks. The dbt activity is positioned for dependency-aware execution with runtime parameters, notifications, and centralized monitoring of runs, which helps when you want a single pipeline to say: ingest -> transform (dbt) -> validate -> publish training tables -> kick off model training/evaluation. For teams already invested in dbt for transformation logic, this reduces context switching and helps standardize operational controls (parameterization, retries, run visibility) around the transformation stage that typically feeds ML.
- [Nested folders support in shortcut transformations (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/nested-folders-support-in-shortcut-transformations-generally-available/)
- [Orchestrate dbt jobs activity in your Fabric pipelines (Preview)](https://blog.fabric.microsoft.com/en-US/blog/orchestrate-dbt-jobs-activity-in-your-fabric-pipelines-preview/)
## Streaming and lakehouse architecture guidance (Eventstream network security, Medallion decision guide)
Two guidance pieces rounded out the week by focusing on the architecture decisions that tend to determine whether an ML platform stays maintainable six months from now. For Fabric Eventstream, Microsoft published a network security decision guide that breaks streaming traffic into internal, inbound, and outbound paths, then maps those scenarios to the right private networking option: managed private endpoints, tenant/workspace Private Link (Azure Private Link), or connector VNet injection. The key takeaway is that "secure streaming" is not one setting, it depends on which direction data moves and which connector is involved, and that planning those paths early avoids rewrites when security teams later require private-only connectivity. The guide also anchors identity considerations around Microsoft Entra ID, which is often where the operational policies (who can publish/consume streams) actually get enforced.
Separately, a medallion framework decision guide provided a practical checklist for implementing Bronze/Silver/Gold layering in a way that holds up under real production pressures. It covers how to decide layer responsibilities, whether loads should be full, delta, or CDC-driven, and how to design metadata-driven pipelines that can evolve. It also digs into operational topics that directly affect ML dataset quality and trustworthiness: schema evolution strategies, idempotency (so reruns do not corrupt curated tables), DAG vs parallel orchestration choices, retries, and observability. Even if you''re not using the exact same tooling stack, the decision points map cleanly onto Fabric and Databricks-style lakehouse implementations where ML workloads depend on consistent, explainable dataset construction.
- [Secure your data streams: How to choose the right network security feature in Eventstream](https://blog.fabric.microsoft.com/en-US/blog/secure-your-data-streams-how-to-choose-the-right-network-security-feature-in-eventstream/)
- [Designing a Medallion Framework — A Decision Guide](https://techcommunity.microsoft.com/t5/azure-architecture-blog/designing-a-medallion-framework-a-decision-guide/ba-p/4514349)',
    'This week in machine learning, the center of gravity was Fabric: Microsoft kept pushing the practical plumbing that turns models into something teams can run repeatedly and safely. The updates focused on tightening the MLOps loop (promoting experiments and models across environments), feeding ML and analytics with fresher data (streaming change events into Fabric), and making data prep more maintainable (better lake folder handling and more orchestration options), with a consistent thread of "do it securely over private networking."',
    1777273200, 'ml', '/ml/roundups/weekly-ml-roundup-2026-04-27', 'TechHub',
    'TechHub', 'B792CAE89E076041D3EE68D031C7DD1BD5605B96977144D2BE9C5C6BC5AA3001', ',Microsoft Fabric,MLOps,MLflow,Synapseml Mlflow,Model Registry,Experiment Tracking,DevTestProd,OneLake,Delta Lake,Eventstream,SQL Change Event Streaming,CloudEvents,Event Hubs,Private Link,Dbt,ML,Roundups,',
    false, false, false, false, false,
    true, false, 32
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
-- weekly-ml-roundup-2026-04-20
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ml-roundup-2026-04-20', 'roundups', 'Weekly Machine Learning Roundup: OneLake Ingestion and Unified Analysis',
    'This week''s ML-adjacent Fabric updates focused on reducing two workflow frictions: getting local artifacts into OneLake, and moving between SQL, notebooks, and KQL analysis without re-learning each workload UI. Building on last week''s "operationalize the platform" theme (safer ingestion, fewer embedded secrets, smoother Warehouse querying), these changes aim to reduce glue work once teams move beyond prototypes.
<!--excerpt_end-->
## Microsoft Fabric: lower-friction ingestion and a more consistent analysis surface
OneLake File Explorer is now GA, addressing a common prototyping need: early datasets and artifacts often start on a developer machine (Excel, CSV, Parquet, images, intermediate outputs). With Windows File Explorer integration, OneLake mounts in Explorer so teams can browse by workspace/item and use standard file operations like drag-and-drop to place files where they belong. In the context of last week''s Eventstreams ingestion and security improvements (private networking, Key Vault certs, fewer embedded connection strings), this is a complementary on-ramp: teams can move local artifacts into governed storage without scripts or portal detours. Once in OneLake, data is immediately usable across Fabric experiences (pipelines, notebooks, semantic models) without one-off uploads during iteration.
In preview, Fabric is reducing UI fragmentation with a unified "Analyze data with" entry point across Lakehouse, Data Warehouse, and Eventhouse. This follows last week''s "cleaner warehouse SQL" thread: once data is shared in OneLake, friction often shifts to inconsistent compute and query entry points. Eventhouse Endpoint now appears alongside SQL Endpoint and Notebook options so switching modalities is predictable from the same menu. For Lakehouse and Warehouse, enabling Eventhouse Endpoint provisions an Eventhouse and KQL Database as child artifacts with backend-managed schema sync, which provides a KQL surface over the same data without manual sync or duplication. That matches last week''s push for managed configuration over bespoke integration. Eventhouse also gets the same menu at the database level (next to Share), and notebook launching is standardized so opening from Eventhouse/KQL Database auto-adds the database to the notebook environment for consistent Spark notebook behavior across workloads.
- [Bring your local files to OneLake with OneLake File Explorer (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/bring-your-local-files-to-onelake-with-onelake-file-explorer-generally-available/)
- [Unifying “Analyze data with” analytics across Fabric (Preview)](https://blog.fabric.microsoft.com/en-US/blog/unifying-analyze-data-with-analytics-across-fabric-preview/)',
    'This week''s ML-adjacent Fabric updates focused on reducing two workflow frictions: getting local artifacts into OneLake, and moving between SQL, notebooks, and KQL analysis without re-learning each workload UI. Building on last week''s "operationalize the platform" theme (safer ingestion, fewer embedded secrets, smoother Warehouse querying), these changes aim to reduce glue work once teams move beyond prototypes.',
    1776668400, 'ml', '/ml/roundups/weekly-ml-roundup-2026-04-20', 'TechHub',
    'TechHub', 'E1D9D4EC5E92884D9380DC1BF0ECCC79B0D6632294699FDA96C7D95D0787528A', ',Microsoft Fabric,OneLake,OneLake File Explorer,Data Ingestion,Governed Storage,Windows File Explorer,Lakehouse,Data Warehouse,Eventhouse,KQL,KQL Database,SQL Endpoint,Spark Notebooks,Schema Synchronization,UI Consistency,ML,Roundups,',
    false, false, false, false, false,
    true, false, 32
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
-- weekly-ml-roundup-2026-04-13
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ml-roundup-2026-04-13', 'roundups', 'Weekly Machine Learning Roundup: MLOps Pipelines and Safer Streaming',
    'This week''s ML thread was about shipping models and data products with fewer operational surprises. Azure ML plus Azure DevOps guidance went deep on repeatable training-to-serving pipelines and the details that tend to break CI/CD. Fabric continued last week''s "operationalize the platform" momentum, focusing this time on real-time ingestion security and smoother warehouse querying to reduce glue work once systems move past prototype.
<!--excerpt_end-->
## Azure Machine Learning + Azure DevOps: a repeatable training-to-endpoint MLOps pipeline
An end-to-end template showed how to take a scikit-learn model from local training to reliable serving on Azure ML Managed Online Endpoints using an Azure DevOps multi-stage YAML pipeline. It splits into four stages (DevOps gate -> Train -> Register -> Deploy) so teams can validate and capture metadata early, retrain only when needed, and rerun register/deploy without retraining after transient failures.
In Train, the example standardizes the environment (Python 3.12), pulls data from Azure Blob Storage (CSV/Parquet via `adlfs`/`pyarrow` patterns), and adds basic validation (schema and row counts) before feature engineering and fitting (with `StandardScaler` as the example preprocessing). The output is one serialized artifact: a pickle bundle containing the estimator, fitted preprocessor, expected feature column order, and metadata (timestamps, row counts, scikit-learn version) to prevent silent mismatches and manage pickle compatibility.
Register uses the Azure ML CLI (`az extension add -n ml`, then `az ml model create`) to push the artifact into an Azure ML Registry, using auto version incrementing for re-registers under the same model name. Deploy then creates/updates a Managed Online Endpoint and deploys a specific model version (example: "blue" with all traffic) using `az ml online-endpoint create/show` and `az ml online-deployment create`, and finishes with a smoke test via `az ml online-endpoint invoke` to confirm the endpoint is callable.
It also covers operational details that determine whether this works in a team setting: managed-endpoint scoring script structure (`init()` loading from `AZUREML_MODEL_DIR`, `run()` enforcing feature order, applying the stored scaler, returning predictions), tradeoffs among `pickle`, `joblib`, and ONNX, and a warning on untrusted pickle deserialization. On DevOps/security, it reinforces no secrets in code (env vars/variable groups), managed identity over keys/secrets, least-privilege RBAC, and sample roles (Storage Blob Data Reader, AzureML Registry User, AzureML Data Scientist), plus workload identity federation from Azure DevOps to a user-assigned managed identity. It also flags pitfalls (Windows agent command differences, checkout behaviors, schema mismatches) and suggests extensions like validation gates, batch endpoints, drift monitoring, environment promotion, and blue/green or traffic-splitting.
- [Building an End-to-End MLOps Pipeline: From Training to Managed Endpoints on Azure](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/building-an-end-to-end-mlops-pipeline-from-training-to-managed/ba-p/4509852)
## Microsoft Fabric: real-time ingestion/security upgrades and cleaner warehouse SQL
Fabric''s ML-adjacent updates focused on improving ingestion and querying that feed scoring, enrichment, and analytics, continuing last week''s push to reduce bespoke operations. Last week emphasized orchestration surfaces and recovery. This week targets streaming ingestion (networking, certs, retries, fewer embedded secrets) and warehouse SQL ergonomics to reduce production friction.
In Eventstreams (Q1 2026 recap), ingestion expanded and Spark handoff tightened. New preview connectors include DeltaFlow for converting DB CDC events (inserts/updates/deletes) into structured streams, which reduces manual CDC format/schema/destination work. MQTT enhancements add v3.1 and v3.1.1 support to onboard existing brokers/device fleets without upgrades. Anomaly Detection also appears as a preview source, making anomaly signals first-class streaming inputs for routing/enrichment with telemetry. For teams following last week''s orchestration theme, these broaden what becomes "just another input" into repeatable pipelines, especially where CDC/anomaly feeds must land reliably before downstream scoring/feature updates.
Eventstreams also improved processing integration with Spark Structured Streaming and Fabric Notebooks to reduce setup friction: discover Eventstreams through Real-Time Hub, auto-generate PySpark connection snippets, and reuse shared notebooks within Eventstreams. Operationally, it pushes safer defaults by reducing embedded connection strings/SAS keys and adding notebook auto-retry policies to restart streaming jobs after failures. This fits last week''s "recover fast" theme by adding resilience settings for long-running streams and reducing secret sprawl.
Enterprise connectivity and security also advanced with preview private network ingestion for VNet/on-prem sources using an Azure managed virtual network bridge, supporting VPN, ExpressRoute, peering, private endpoints, and a streaming VNet data gateway experience. Connector security added preview custom CA certificates and mutual TLS (mTLS), with certs stored in Azure Key Vault for centralized rotation. This is called out for Kafka sources including Apache Kafka, Amazon MSK, Confluent Cloud for Apache Kafka, and Confluent Schema Registry. It matches last week''s "platform-managed" posture: connectivity and cert rotation move into managed config and Key Vault-backed rotation rather than custom code.
Separately, Fabric Data Warehouse shipped GA support for T-SQL `ANY_VALUE()` as an aggregate/analytic function, which addresses a common reporting and semantic-layer pain point. It returns an arbitrary representative value per `GROUP BY` group (or window partition) when projected columns are functionally dependent on the grouping key. For example, you can group revenue by `GeographyID` while including `City`, `State`, `Country` without expanding the `GROUP BY`. It is clearer than `MIN()`/`MAX()` workarounds and can reduce unnecessary grouping columns, with the guardrail that it is only valid when values are constant in the group. Paired with last week''s recovery work, it is another everyday production SQL/ops edge being improved.
- [What’s new in Fabric Eventstream: 2026 Q1 Edition](https://blog.fabric.microsoft.com/en-US/blog/whats-new-in-fabric-eventstream-2026-q1-edition/)
- [Use ANY_VALUE() for simpler grouping of results in Fabric Data Warehouse (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/use-any_value-for-simpler-grouping-of-results-in-fabric-data-warehouse-generally-available/)',
    'This week''s ML thread was about shipping models and data products with fewer operational surprises. Azure ML plus Azure DevOps guidance went deep on repeatable training-to-serving pipelines and the details that tend to break CI/CD. Fabric continued last week''s "operationalize the platform" momentum, focusing this time on real-time ingestion security and smoother warehouse querying to reduce glue work once systems move past prototype.',
    1776063600, 'ml', '/ml/roundups/weekly-ml-roundup-2026-04-13', 'TechHub',
    'TechHub', 'B532BEE4A747EDD7D4655A66821E56D285688E30A7F268F6DD2EA12EC950B539', ',ML,Mlops,Azure Machine Learning,Azure DevOps,CI CD,Managed Online Endpoints,Azure ML CLI,Model Registry,Managed Identity,Workload Identity Federation,Microsoft Fabric,Eventstreams,Spark Structured Streaming,Azure Key Vault,T SQL,Roundups,',
    false, false, false, false, false,
    true, false, 32
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
-- weekly-ml-roundup-2026-04-06
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ml-roundup-2026-04-06', 'roundups', 'Weekly Machine Learning Roundup: Fabric orchestration and guardrails',
    'This week’s Fabric updates focused on production gaps for data and ML-adjacent workloads: more standard orchestration (especially for Airflow teams) and more day-2 guardrails via alerting and recovery to reduce downtime from failures or deletes. This continues last week’s "managed operating surfaces" thread, where dbt Jobs, Activator-triggered actions, and improved diagnostics emphasized repeatable, observable workflows.
<!--excerpt_end-->
## Microsoft Fabric orchestration and operations (Airflow, scheduling, recovery)
Fabric Data Factory’s Apache Airflow integration added native operators to run Fabric artifacts directly from Airflow DAGs. Teams can invoke Fabric Notebooks, Spark job definitions, Fabric pipelines, Semantic Models, and user data functions as first-class tasks, with broader coverage including Copy jobs and dbt jobs. This builds on last week’s emphasis on dbt Jobs as a scheduling/observability plane and Copy job improvements for incremental/CDC ingestion, but it now lets existing Airflow standards orchestrate those Fabric primitives without custom glue. It also complements last week’s Activator direction (event -> action inside Fabric) by giving teams another coordination surface when a DAG view is preferred.
Fabric also added a shortcut, "Run Fabric Artifact" in the Airflow job context menu, that inserts the needed code/config to call a Fabric item. This speeds DAG authoring and reduces boilerplate, which matches the recent push to minimize bespoke integration code.
New Apache Airflow job APIs also support platform automation: programmatic management/monitoring/triggering of DAG runs from external services, including event-driven scenarios. This fits teams integrating Fabric orchestration with CI/CD, internal portals, or runbooks, and matches last week’s API-first posture (dbt Jobs APIs, workspace tags via REST, Notebook Public APIs referenced previously). The direction is increasingly "everything is addressable as an API," which supports consistent promotion, scheduling, and monitoring across many workspaces.
Operationally, Fabric improved both "find out fast" and "recover fast." Scheduled job failure notifications are now GA: configure recipients once per item under Schedule settings, and the list applies to all schedules for that item. Failed scheduled runs email error details plus a link to the Monitoring Hub run, and it works for schedules created in the UI or via the Job Scheduler REST API. The limitation is explicit: only scheduled runs trigger emails, not manual runs, so ad-hoc execution still needs separate practices. This extends last week’s day-2 manageability theme by making managed schedules more actionable without constant dashboard watching.
Fabric Data Warehouse also added preview recovery for a dropped warehouse via the workspace Recycle Bin. Within a tenant-set retention window (7-90 days, default 7), a Workspace Admin can restore a warehouse to its pre-delete state, including schemas/data, snapshots, permissions/security settings, and objects like saved queries, views, and stored procedures. For fast-moving environments, this is a cleaner rollback than rebuilding and replaying pipelines, and it pairs with last week’s "productionize the plumbing" theme by reducing blast radius when mistakes happen.
- [Announcing the latest innovations in Fabric Data Factory: Apache Airflow jobs and pipelines](https://blog.fabric.microsoft.com/en-US/blog/announcing-the-latest-innovations-in-fabric-data-factory-apache-airflow-jobs-and-pipelines/)
- [Get notified when scheduled jobs fail in Fabric (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/get-notified-when-scheduled-jobs-fail-in-fabric-generally-available/)
- [Dropped warehouse recovery in Microsoft Fabric (Preview)](https://blog.fabric.microsoft.com/en-US/blog/dropped-warehouse-recovery-in-microsoft-fabric-preview/)',
    'This week’s Fabric updates focused on production gaps for data and ML-adjacent workloads: more standard orchestration (especially for Airflow teams) and more day-2 guardrails via alerting and recovery to reduce downtime from failures or deletes. This continues last week’s "managed operating surfaces" thread, where dbt Jobs, Activator-triggered actions, and improved diagnostics emphasized repeatable, observable workflows.',
    1775458800, 'ml', '/ml/roundups/weekly-ml-roundup-2026-04-06', 'TechHub',
    'TechHub', '1378CD0A353F8F005FE13E5002C4FF4DEBF259EEAD78DEF2D11EE09AB7B1267C', ',Microsoft Fabric,Fabric Data Factory,Apache Airflow,Orchestration,Scheduling,DAGs,Notebooks,Spark,Dbt,Pipelines,Semantic Models,APIs,Monitoring Hub,Job Failure Notifications,Data Warehouse Recovery,ML,Roundups,',
    false, false, false, false, false,
    true, false, 32
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
-- weekly-ml-roundup-2026-03-30
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ml-roundup-2026-03-30', 'roundups', 'Weekly Machine Learning Roundup: Fabric ops, dbt, and event actions',
    'This week''s ML-adjacent momentum mostly came through Microsoft Fabric, with updates that make analytics engineering more like a managed product: repeatable transformation workflows (dbt), more event-driven automation (Activator + UDFs), and steadier ingestion mechanics (Copy job upgrades, more connectors, easier troubleshooting). Building on last week''s "pipelines over one-off notebooks" theme (Materialized Lake Views, Environments, Notebook Public APIs), the thread is Fabric turning those building blocks into managed operating surfaces: author in familiar tools, execute in Fabric, and connect actions with less custom glue. Fabric also tightened admin/governance with better workspace organization at scale.
<!--excerpt_end-->
## Microsoft Fabric’s dbt roadmap: adapters, operational dbt Jobs, and a path to Fusion
Fabric continues treating dbt as a first-class workflow, focusing not just on adapter availability but on correctness for Fabric SQL semantics, materializations, and performance. This mirrors last week''s shift toward declarative transforms via Materialized Lake Views: dbt is another "transformations as code" path, and Fabric is aiming for clean mapping to Warehouse and (soon) Lakehouse execution. Today, the recommendation for SQL-first managed warehouse work is the Fabric Warehouse dbt core adapter; a Fabric Lakehouse dbt core adapter is "coming soon" as GA for running dbt directly on Lakehouse tables in OneLake, aligned with Fabric governance and compute/storage separation.
Operationally, dbt Jobs in Fabric (public preview since December 2025) is positioned as the control plane for scheduling, retries, environment promotion, and observability. This matches last week''s "managed orchestration" focus (Notebook Public APIs + Job Scheduler): less interactive execution, more managed jobs with traceable outputs. Recent additions include public package support, native GitHub support (run jobs from GitHub-hosted dbt projects for CI/CD alignment), and OneLake-based enterprise logging with no size limits (removing the prior 1 MB cap). API support enables automation, and "coming soon" items include dbt Jobs as a Fabric Pipelines activity with parameterization, plus Lakehouse adapter support in dbt Jobs (Warehouse supported today).
Looking ahead, Fabric called out planned dbt Fusion support expected later in calendar Q2 2026, focusing on clean Warehouse/Lakehouse adapter integration and aligned execution metadata/observability as Fusion enters dbt''s runtime story. The net effect is a cohesive path: author in GitHub, execute/schedule in Fabric, centralize logs in OneLake, and adopt Fusion-backed execution later without reworking Warehouse/Lakehouse layouts.
- [dbt + Microsoft Fabric: dbt adapters, dbt Jobs on OneLake, and upcoming dbt Fusion support](https://blog.fabric.microsoft.com/en-US/blog/dbt-microsoft-fabric-a-strategic-investment-in-the-modern-analytics-stack/)
## Fabric Real-Time Intelligence: Activator grows from alerting into action (Teams, Spark, Dataflows, and UDF triggers)
Fabric Activator is expanding from "tell me something happened" to "do something when it happens," adding rule actions to send Microsoft Teams messages and trigger compute/pipeline work: run a Spark job, run a User Data Function (UDF), or run a Dataflow (Dataflows Gen2). This reduces glue code by removing the need for custom listener services that translate events into downstream work, especially when teams want event-driven processing instead of scheduled refresh. It follows last week''s automation direction: after notebooks became easier to run/manage via APIs, Activator now provides an "event -> execution" surface inside Fabric without external schedulers.
Two additions stand out for operational workflows. First, triggering UDFs from Activator creates a direct event-to-function bridge: rules can pass entity IDs, values, and timestamps into code, enabling incidents/runbooks/custom logic without new infrastructure. This pairs with this week''s UDF defaults update: as UDFs become shared primitives invoked by rules, backwards-compatible signatures matter more. Second, Spark job and Dataflow actions can respond to Fabric and Azure Blob Storage events, enabling "data landed, process now" patterns rather than waiting for schedules, similar in spirit to last week''s near-real-time pipeline patterns but implemented through Fabric''s event/action model.
Authoring surfaces broadened too: Warehouse SQL query monitoring rules (Preview) let rules run on ad-hoc or saved query results on a frequency, and Ontology entity rules (Preview) support entity-level conditions. Rule creation is now embedded in Eventstream, and Power BI integration improved so Activator can alert when a new row appears in a table visual in a published report, which helps when dashboards function as queue views.
- [What’s new with Fabric Activator: more connected and capabilities](https://blog.fabric.microsoft.com/en-US/blog/whats-new-with-fabric-activator-more-connected-and-capabilities/)
## Fabric Data Factory: Copy job and connector upgrades for incremental movement, CDC, and cross-cloud destinations
Fabric Data Factory''s Copy job updates targeted ingestion constraints where schemas do not match ideal assumptions. This is Fabric''s version of the "productionize the plumbing" story we touched last week (Databricks Lakeflow simplifying ingestion + CDC + SCD): in Fabric, improvements are landing in Copy job incremental and CDC behavior, which often blocks teams before transformations like MLVs or dbt. Incremental copy is now more flexible in GA with additional watermark types: ROWVERSION, date/datetime (with delayed extraction to reduce missed late updates), and string columns interpreted as datetime. This reduces custom query workarounds while still using built-in state tracking and checkpointing.
CDC replication added three practical updates: Oracle as a CDC source, Fabric Data Warehouse as a CDC sink, and an SCD Type 2 write method in Preview as a simple toggle. The SCD2 option provides history-table semantics (new version rows on updates; soft deletes via expiring current versions), reducing per-table MERGE logic and custom frameworks. It echoes last week''s SCD2-as-first-class capability in Databricks, but here it''s pushed down into ingestion so history tables can be created earlier without bespoke transform code.
Connector and throughput improvements also landed. SharePoint Online File is now GA as source/destination, easing "files in SharePoint" ingestion/publishing. BigQuery, MySQL, and PostgreSQL gained destination write support in Preview for more cross-cloud movement. "Native incremental copy" expanded to more connectors (including RDS variants, ODBC, GCS, SharePoint Lists/Files, Fabric Lakehouse tables/files), and automatic partitioning was introduced to speed large-table loads by parallelizing reads/writes via a selected partition column without manual setup.
- [Incremental copy gets more flexible—New watermark column types in Copy job in Fabric Data Factory (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/incremental-copy-gets-more-flexible-new-watermark-column-types-in-copy-job-in-fabric-data-factory-generally-available/)
- [Richer CDC in Fabric Data Factory Copy job: Oracle source, Fabric Data Warehouse sink, and SCD Type 2 (Preview)](https://blog.fabric.microsoft.com/en-US/blog/simplifying-data-movement-across-multiple-clouds-with-richer-cdc-in-copy-job-in-fabric-data-factory-oracle-source-fabric-data-warehouse-sink-and-scd-type-2-preview/)
- [Outstanding connectivity for data movement in Fabric Data Factory](https://blog.fabric.microsoft.com/en-US/blog/outstanding-connectivity-for-data-movement-in-fabric-data-factory/)
## Other ML News
Fabric''s programmable surfaces got a small but useful update: User Data Functions (UDFs) now support default arguments in Python. Because inputs are JSON-serialized, defaults must be JSON-serializable (strings, numbers, booleans, arrays/lists, objects/dicts, and datetime-like strings, ideally ISO 8601). The guidance also reiterates standard Python practice for mutable defaults (use `None` then assign inside), which helps teams evolve shared UDFs without breaking callers. This pairs with Activator triggering UDFs: defaults allow signature extension without updating every rule immediately.
- [Support for default arguments in Fabric User data functions](https://blog.fabric.microsoft.com/en-US/blog/support-for-default-arguments-in-fabric-user-data-functions/)
Dataflow Gen2 troubleshooting is becoming more self-service. A Preview feature lets admins/support download a per-run diagnostic package from run history after completion. It bundles metadata, structured logs, execution traces, and runtime/environment signals, reducing time spent collecting evidence across views for failed or slow runs. This continues last week''s day-2 manageability thread: as more execution becomes managed and event-driven, diagnostics determine whether failures are quickly explainable.
- [Dataflow Gen2 – Dataflow Diagnostics Download (Preview)](https://blog.fabric.microsoft.com/en-US/blog/dataflow-gen2-dataflow-diagnostics-download-preview/)
Workspace tags are now GA, providing a first-class way to label workspaces (team, project, environment, cost center) and filter them in the workspaces list and OneLake Catalog Explorer. Tags are also exposed via REST APIs (create/apply/remove and included in Get/List Workspaces), supporting automated inventory and governance reporting; Fabric Scanner APIs are expected to include tags later. This complements last week''s API-driven ops push: as teams automate notebook/job lifecycles, programmatic workspace organization helps control sprawl.
- [Find and manage workspaces faster with workspace tags (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/find-and-manage-workspaces-faster-with-workspace-tags-generally-available/)
Fabric Open Mirroring added a GA ERP replication option: the BC2Fab Fabric Workload (Navida) replicates Dynamics 365 Business Central tables into Fabric with incremental change detection and schema evolution handling. The goal is lighter transformation-heavy ingestion and reduced load on production ERP, while enabling querying in Fabric engines and Power BI reporting on OneLake-backed copies. Like last week''s consolidation of ingestion and governance for near-real-time pipelines, it continues moving replication closer to standardized OneLake landing zones so downstream dbt/MLV work can focus on shaping data, not extraction.
- [Integrating Dynamics 365 Business Central with Microsoft Fabric using Open Mirroring with BC2Fab workload (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/integrating-dynamics-365-business-central-with-microsoft-fabric-using-open-mirroring-with-bc2fab-workload-generally-available/)',
    'This week''s ML-adjacent momentum mostly came through Microsoft Fabric, with updates that make analytics engineering more like a managed product: repeatable transformation workflows (dbt), more event-driven automation (Activator + UDFs), and steadier ingestion mechanics (Copy job upgrades, more connectors, easier troubleshooting). Building on last week''s "pipelines over one-off notebooks" theme (Materialized Lake Views, Environments, Notebook Public APIs), the thread is Fabric turning those building blocks into managed operating surfaces: author in familiar tools, execute in Fabric, and connect actions with less custom glue. Fabric also tightened admin/governance with better workspace organization at scale.',
    1774854000, 'ml', '/ml/roundups/weekly-ml-roundup-2026-03-30', 'TechHub',
    'TechHub', '6391B41CDAB34673CBCF8593224EBDA2F6B272DA7DDDF6822E77E85CBB49FCD2', ',Microsoft Fabric,Dbt,Dbt Jobs,Dbt Fusion,OneLake,Fabric Warehouse,Fabric Lakehouse,Real Time Intelligence,Fabric Activator,User Data Functions,Spark,Dataflows Gen2,Fabric Data Factory,Copy Job,Change Data Capture,ML,Roundups,',
    false, false, false, false, false,
    true, false, 32
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
-- weekly-ml-roundup-2026-03-23
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ml-roundup-2026-03-23', 'roundups', 'Weekly Machine Learning Roundup: Lakehouse Pipelines and Governance',
    'This week''s ML-adjacent data engineering updates were less about model releases and more about tightening pipelines and developer surfaces. Fabric moved Spark and notebook capabilities closer to production usage, and Azure Databricks shared a concrete pattern for consolidating near-real-time ingestion, transformation, and governance into a single Lakeflow workflow.
<!--excerpt_end-->
## Microsoft Fabric: Lakehouse pipelines, runtimes, and notebook automation
Materialized Lake Views (MLVs) GA is Fabric''s clearest move toward declarative lakehouse transforms without hand-rolled Spark ETL plus separate orchestration. The GA release focuses on refresh behavior and manageability: Fabric expands incremental refresh support across common patterns (aggregations with `GROUP BY`, left outer joins, left semi joins, CTEs) and decides per run whether incremental or full recompute is cheaper based on change volume and estimated cost. With Change Data Feed enabled by default for new MLVs, incremental processing becomes the default rather than another setting. Operationally, multi-schedule support at the lakehouse level lets you define named schedules for subsets of views (hourly gold vs six-hour lower-priority), with Fabric handling dependencies, parallelizing independent views, and centralizing errors; overlapping triggers are skipped if a refresh is already running. "Replace" allows updating an MLV definition in place without drop/recreate, preserving identity/metadata/lineage and avoiding broken dependencies. Data quality constraints get fuller reporting across refresh history, including richer expression-based constraints for PySpark-authored MLVs (multi-column expressions, arithmetic/functions, session-scoped Python UDFs), while PySpark authoring itself is preview; PySpark-authored MLVs still full-refresh for now.
Fabric Runtime 2.0 entered preview as a new baseline for Spark engineering and science workloads, and it is the kind of upgrade that usually requires retesting. It brings Spark 4.0, Delta 4.0, Azure Linux 3.0 (Mariner 3.0), Java 21, Scala 2.13, and Python 3.12, with Spark 4.1 / Delta 4.1 / Python 3.13 planned soon. Because you can enable preview at workspace or environment level, teams can stage rollout: validate connector JARs with Scala 2.13, check Java 21 requirements, and confirm Python wheels/native dependencies before moving to production.
Fabric also added guidance to make Spark work more reproducible. Environments best practices splits workflows into Quick mode (fast publish, installs at session start, good for iteration and testing overrides) and Full mode (3-6 minute publish for a validated snapshot, then 1-3 minute session startup), with a practical middle ground: Full mode plus a custom live pool for reproducibility with ~5s session startup. It also recommends using Resources folder/inline installs only for early-stage or one-off work, then promoting validated dependencies into Full mode for scheduled jobs and shared production runs.
Notebook automation is also becoming a first-class integration surface with Fabric Notebook Public APIs GA. Teams can manage notebooks via REST (create/update/get/list/delete) and execute them through the Fabric Job Scheduler API with parameters, session config, and explicit execution context (environment/lakehouse). Two details matter for CI/CD-style orchestration: service principal auth for unattended automation, and the Run Notebook API returning an exit value (via notebook utilities) so external orchestrators can branch or gate on structured output, not just success/failure. Together, the story is coherent: define transforms declaratively (MLVs), adopt a new Spark/Delta baseline when ready (Runtime 2.0), control dependencies (Environments), and orchestrate notebooks via APIs.
- [Materialized Lake Views in Microsoft Fabric (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/materialized-lake-views-in-microsoft-fabric-generally-available/)
- [Fabric Runtime 2.0 (Preview)](https://blog.fabric.microsoft.com/en-US/blog/fabric-runtime-2-0-preview/)
- [Best Practices for Library Management with Fabric Environments](https://blog.fabric.microsoft.com/en-US/blog/33772/)
- [Fabric Notebook Public APIs (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/fabric-notebook-public-apis-generally-available/)
## Azure Databricks Lakeflow: near-real-time ingestion, SCD transformations, and governance in Delta Lake
Azure Databricks published a detailed walkthrough for collapsing "too many tools" into a Lakeflow-native pipeline, covering ingestion, transformation, orchestration, monitoring, lineage, and Unity Catalog access control. The architecture starts with two Bronze ingestion paths into Delta: application telemetry streamed into Delta via Lakeflow Connect "Zerobus Ingest" over gRPC, and SQL Server CDC ingested incrementally from an on-prem transaction log (assuming ExpressRoute). For telemetry, it includes concrete prerequisites (Unity Catalog + serverless), SQL for creating UC tables (for example, `prod.bronze.telemetry_events`), and service principal grants (`GRANT USE CATALOG/SCHEMA` plus `GRANT MODIFY, SELECT`). It shows deriving a Zerobus endpoint from workspace URL (`<workspace-id>.zerobus.<region>.azuredatabricks.net`) and Python using `databricks-zerobus-ingest-sdk` to stream client-credential auth, define JSON record types, ingest records, and close streams, with targets like sub-5s latency and up to ~100 MB/sec per connection; records become queryable immediately via Unity Catalog.
For SQL Server CDC, the focus is correctness and incremental efficiency: TCP 1433 connectivity, enabling CDC with `sys.sp_cdc_enable_db` / `sys.sp_cdc_enable_table`, plus SQL permissions (CDC read) and Databricks privileges (`CREATE CONNECTION` at metastore, plus destination `USE CATALOG` / `CREATE TABLE`). Setup then uses the Databricks UI (Data Ingestion -> Add Data): configure an ingestion gateway, connection details, select tables, optionally enable SCD Type 2 history per table, map outputs to Bronze tables (`orders_raw`, `customers_raw`), and schedule runs (example every 5 minutes).
Transformations use Lakeflow Spark Declarative Pipelines and a medallion pattern with SQL-defined incremental processing: `CREATE OR REFRESH STREAMING TABLE` for Silver; `APPLY CHANGES INTO` keyed with `SEQUENCE BY updated_at` for SCD Type 1 "latest state" and SCD Type 2 customer history; telemetry data quality constraints with `EXPECT ...` and violating rows dropped. Gold uses `CREATE OR REFRESH MATERIALIZED VIEW` joining orders/customers/telemetry and aggregating metrics (including conditional sums like purchase event counts). Continuous mode keeps it near real time; Unity Catalog registers everything so lineage flows from Gold back to Bronze automatically. Governance details include granting analysts access only to Gold and applying PII masking via UDF (for example, `mask_email`) that reveals full data only for privileged groups, enforced with `ALTER TABLE ... ALTER COLUMN ... SET MASK`.
Orchestration and monitoring use Lakeflow Jobs to chain dependencies (CDC ingestion then transforms) with scheduling and notifications. For day-2 operations, it shows querying system tables like `system.lakeflow.job_run_timeline` for runs, states, and durations. Consumption examples stay inside Databricks (AI/BI Dashboards and Genie NL->SQL) while relying on Unity Catalog permissions, keeping access control and lineage consistent for BI and ML feature preparation off the same Gold layer.
- [Near–Real-Time CDC to Delta Lake for BI and ML with Lakeflow on Azure Databricks](https://techcommunity.microsoft.com/t5/azure-databricks/near-real-time-cdc-to-delta-lake-for-bi-and-ml-with-lakeflow-on/ba-p/4502750)
## Other ML News
Fabric Eventhouse (Real-Time Intelligence) previewed a small but useful KQL workflow update: DB Explorer can browse stored functions, show definitions read-only, and run "Preview results" without manually writing the KQL call (including parameter formatting). Parameter prompts plus a 100-row preview cap make it a quick validation step when iterating on function libraries or reviewing inherited functions before using them in dashboards and reports.
- [''Instantly Run and Preview Functions in Microsoft Fabric Eventhouse: No Code Required''](https://blog.fabric.microsoft.com/en-US/blog/instantly-run-and-preview-functions-in-microsoft-fabric-eventhouse-no-code-required/)',
    'This week''s ML-adjacent data engineering updates were less about model releases and more about tightening pipelines and developer surfaces. Fabric moved Spark and notebook capabilities closer to production usage, and Azure Databricks shared a concrete pattern for consolidating near-real-time ingestion, transformation, and governance into a single Lakeflow workflow.',
    1774252800, 'ml', '/ml/roundups/weekly-ml-roundup-2026-03-23', 'TechHub',
    'TechHub', '7EDE86A65565E82E32649BDFC539B0DA1360FA9B24F599B189BFDF9A3421CD84', ',Microsoft Fabric,Azure Databricks,Lakeflow,Delta Lake,Apache Spark,Unity Catalog,Change Data Feed,Materialized Lake Views,SCD Type 2,Medallion Architecture,Notebook Automation,REST APIs,KQL,Data Quality Constraints,ML,Roundups,',
    false, false, false, false, false,
    true, false, 32
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
-- weekly-ml-roundup-2026-03-09
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ml-roundup-2026-03-09', 'roundups', 'Weekly Machine Learning Roundup: LLM Inference and Multimodal Reasoning',
    'This week''s ML section covers improvements in large language model (LLM) deployment, multimodal AI, and changes to enterprise patterns on Microsoft’s cloud stack. It includes guides on inference efficiency, permission updates, and releases of new AI models.
<!--excerpt_end-->
## LLM Inference Optimization and Architecture on Azure
The Azure ML updates highlight resources for selecting the best trade-offs between prediction accuracy, request latency, and budget, using AKS, Ray Serve, and vLLM. Articles explain technical measures for improving throughput and scaling, such as TTFT, TPOT, batching, quantization, and memory handling. Fine-grained GPU allocation, modular LLM architecture, and best-fit machine selection are covered. Security and compliance remain fundamental for practical deployment.
- [Part 1: Inference at Enterprise Scale—Managing LLM Tradeoffs in Azure](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/part-1-inference-at-enterprise-scale-why-llm-inference-is-a/ba-p/4498754)
- [The LLM Inference Optimization Stack: A Playbook for Enterprise Teams on Azure](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/the-llm-inference-optimization-stack-a-prioritized-playbook-for/ba-p/4498818)
- [Inference at Enterprise Scale: Why LLM Inference Is a Capital Allocation Problem](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/inference-at-enterprise-scale-why-llm-inference-is-a-capital/ba-p/4498754)
- [Enterprise-Scale Inference on Azure: Architecting for Cost, Latency, and Efficiency](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/inference-at-enterprise-scale-architecting-for-cost-latency-and/ba-p/4498754)
## Multimodal and Vision Reasoning AI: Phi-4-Reasoning-Vision-15B
Microsoft’s Phi-4-Reasoning-Vision-15B model supports new use cases in image-based reasoning, GUI automation, and chart/document analysis. The model’s `thinking_mode` can be adjusted for different latency/quality profiles. Benchmarks show reliable math and object inference. Training insights focus on reinforcement learning, verifier agents, and real-world use cases.
- [Phi-4-Reasoning-Vision-15B: In-Depth Overview and Use Cases](https://techcommunity.microsoft.com/t5/microsoft-developer-community/phi-4-reasoning-vision-15b-use-cases-in-depth/ba-p/4499210)
- [Microsoft Research Unveils Phi-4-Reasoning-Vision-15B Model and Training Insights](https://www.microsoft.com/en-us/research/blog/phi-4-reasoning-vision-and-the-lessons-of-training-a-multimodal-reasoning-model/)
## .NET AI Agent Architecture and Enterprise Patterns
A .NET AI Community Standup session highlights modern agent frameworks, orchestration, and continuous integration/monitoring. It features the Interview Coach sample and introduces MCP and Aspire tooling for production use. The presentation builds on modularization and cloud-native deployment, providing a blueprint for agents in .NET production settings.
- [.NET AI Community Standup: Real-World AI Agent Architecture in .NET](/ai/videos/net-ai-community-standup-real-world-ai-agent-architecture-in-net)
## Other ML News
Read-only permissions are now required to use semantic models with Fabric data agents, while more advanced actions remain behind Build or workspace member roles. This reduces friction and makes collaboration easier for new data modeling projects.
- [Updated Permission Requirements for Semantic Models in Fabric Data Agents](https://blog.fabric.microsoft.com/en-US/blog/update-to-required-permissions-for-semantic-models-in-fabric-data-agents/)',
    'This week''s ML section covers improvements in large language model (LLM) deployment, multimodal AI, and changes to enterprise patterns on Microsoft’s cloud stack. It includes guides on inference efficiency, permission updates, and releases of new AI models.',
    1773043200, 'ml', '/ml/roundups/weekly-ml-roundup-2026-03-09', 'TechHub',
    'TechHub', '13D22812B27A18F01BE01AA6F566B9D7292D16B987599892C79ABC811CD3BDCF', ',ML,LLM Inference,Azure,AKS,Ray Serve,Vllm,GPU Optimization,Quantization,Batching,Latency,Multimodal AI,Computer Vision,Phi 4 Reasoning Vision 15b,.NET AI Agents,Microsoft Fabric,Roundups,',
    false, false, false, false, false,
    true, false, 32
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
-- weekly-ml-roundup-2026-03-02
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ml-roundup-2026-03-02', 'roundups', 'Weekly Machine Learning Roundup: Faster Pipelines, Smarter Agents',
    'Microsoft is rolling out updates to improve analytics pipelines, automate agent training, and streamline data prep. This work covers the entire journey from pipeline optimization and data engineering to advanced real-world deployment of AI agents.
<!--excerpt_end-->
## Vectorized Execution and Data Preparation in Microsoft Fabric
Microsoft Fabric introduces a vectorized C++ execution layer under Apache Spark, bypassing the JVM for faster performance. Technologies like Velox and Gluten route supported Spark jobs directly to the new backend, delivering up to 6× faster batch execution and reduced compute costs. Features are enabled through familiar Spark APIs, with adaptive execution and column pruning. Unsupported tasks still use JVM execution, and Spark Advisor assists with performance monitoring and diagnostics.
Dataflow Gen2 in Fabric offers Recent Data recall, storing access history for files and tables so developers can easily revisit important sources. Automated source discovery and easy folder browsing minimize manual navigation, so teams focus on transformation work. Both features aim to support more productive and responsive data engineering workflows.
- [Under the Hood: An Introduction to the Native Execution Engine for Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/under-the-hood-an-introduction-to-the-native-execution-engine-for-microsoft-fabric/)
- [Recent Data Feature: Streamlining Data Preparation in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/recent-data-get-back-to-your-data-faster-in-fabric-preview/)
## Scalable Multimodal Agents and Recommender Systems
Engineering teams at Microsoft share methods for improving robustness and scaling of multimodal AI agents. Production RL can struggle with stability and reward design, so five approaches are recommended: staged curricula, adaptive reward segmentation, gradient normalization, constraint shaping, and mixed-horizon training. These enable better live agent performance, more reliable coding tasks, and orchestration at scale.
GenRec Direct Learning (DirL) updates move traditional ranking out of feature engineering pipelines by forming unified token embeddings for users, items, and context. New models apply multi-task heads and attention mechanisms for direct generative ranking, simplifying real-time recommendations and providing code examples for batch scaling.
Research also addresses RL for multimodal agents and automated verification tools, supporting complex audio, visual, and document workflows, and improving workflow automation.
- [Engineering and Algorithmic Interventions for Large-Scale Multimodal Agent Post-Training at Microsoft](https://devblogs.microsoft.com/engineering-at-microsoft/engineering-and-algorithmic-interventions-for-multimodal-post-training-at-microsoft-scale/)
- [GenRec Direct Learning: Moving Ranking from Feature Pipelines to Token-Native Sequence Modeling](https://techcommunity.microsoft.com/t5/microsoft-developer-community/genrec-direct-learning-moving-ranking-from-feature-pipelines-to/ba-p/4494252)
- [AI Agents Get Smarter at Juggling Tasks with Microsoft Research Advances](https://www.microsoft.com/en-us/research/blog/corpgen-advances-ai-agents-for-real-work/)',
    'Microsoft is rolling out updates to improve analytics pipelines, automate agent training, and streamline data prep. This work covers the entire journey from pipeline optimization and data engineering to advanced real-world deployment of AI agents.',
    1772438400, 'ml', '/ml/roundups/weekly-ml-roundup-2026-03-02', 'TechHub',
    'TechHub', 'A951221287AF69A6D37B714DE5FDD7BA51F52AB3037EEFA435F7470B77B6B77A', ',ML,Microsoft Fabric,Apache Spark,Vectorized Execution,Native Execution Engine,C++,JVM,Velox,Gluten,Adaptive Query Execution,Column Pruning,Dataflow Gen2,Reinforcement Learning,Multimodal Agents,Recommender Systems,Roundups,',
    false, false, false, false, false,
    true, false, 32
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
-- weekly-ml-roundup-2026-02-23
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ml-roundup-2026-02-23', 'roundups', 'Weekly Machine Learning Roundup: Fabric ODBC and Lakehouse Access',
    'Microsoft releases its preview ODBC driver for Fabric Data Engineering, making it easier to connect enterprise analytics platforms and Spark SQL in Microsoft Fabric. This driver simplifies query capabilities and integrates with analytics and lakehouse solutions.
<!--excerpt_end-->
## Microsoft ODBC Driver for Fabric Data Engineering Preview
The new driver supports Spark SQL in Fabric Data Engineering, allowing .NET and Python integration for managing and querying data via OneLake. The release is ODBC 3.x compliant and works with Fabric’s security and configuration options, including credentials, tokens, certificates, and CLI authentication. Features such as session reuse, async prefetch, and proxy support enhance automation and real-time analytics
By connecting traditional tools and ML workflows, this driver supports lakehouse queries, remote ML scenarios, and analytics, and is shaped by feedback from early developer uses.
- [Introducing the Microsoft ODBC Driver for Fabric Data Engineering (Preview)](https://blog.fabric.microsoft.com/en-US/blog/microsoft-odbc-driver-for-microsoft-fabric-data-engineering-preview/)',
    'Microsoft releases its preview ODBC driver for Fabric Data Engineering, making it easier to connect enterprise analytics platforms and Spark SQL in Microsoft Fabric. This driver simplifies query capabilities and integrates with analytics and lakehouse solutions.',
    1771833600, 'ml', '/ml/roundups/weekly-ml-roundup-2026-02-23', 'TechHub',
    'TechHub', '8123613ED4D35361832A76458DC5D11D2AAA49C4917C148020293E1684758205', ',ML,Microsoft Fabric,Fabric Data Engineering,ODBC,Spark SQL,OneLake,Lakehouse,Enterprise Analytics,Data Engineering,Python,.NET,Authentication,Security,Async Prefetch,Automation,Roundups,',
    false, false, false, false, false,
    true, false, 32
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
-- weekly-ml-roundup-2026-02-16
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ml-roundup-2026-02-16', 'roundups', 'Weekly Machine Learning Roundup: Fabric ML and LLM Fine-Tuning',
    'This week’s ML coverage spotlights Microsoft Fabric’s expansion of analytics and machine learning, with practical routes for ML in production, fine-tuning workflows, and automated pipelines. Tools like Semantic Link and Foundry fine-tuning offer easier, AI-driven analytics and operational intelligence.
<!--excerpt_end-->
## Microsoft Fabric for ML, AI, and Operational Analytics
Microsoft Fabric now better unifies analytics, machine learning, and business reporting.
Semantic Link is now generally available, allowing a shared semantic layer for data engineering, AI, and BI to use common models. It supports semantic model updates directly from notebooks, immediate sync to Power BI, and harmonized workflows. Automation is easier with tighter SQL/Spark orchestration, while community repositories provide reusable patterns.
For IoT and streaming data, Fabric’s operational analytics uses time series dashboards with Kusto, dynamic slicing, anomaly detection, and DirectQuery for live reporting. These updates expand the platform’s ability to handle large-scale, real-time data.
ML workflows in Fabric and Power BI are progressing, letting teams run predictions in dashboards using LightGBM/SMOTE, OneLake-backed data, and MLflow for automation. The Fabric IQ platform provides the foundation for digital twins and ontologies, supporting smarter knowledge and automation development.
- [Supercharge AI, BI, and Data Engineering with Semantic Link in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/supercharge-ai-bi-and-data-engineering-with-semantic-link-generally-available/)
- [Adaptive Time Series Visualization at Scale with Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/adaptive-time-series-visualization-at-scale-with-microsoft-fabric/)
- [Integrating Machine Learning with Power BI Reports in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/enrich-power-bi-reports-with-machine-learning-in-microsoft-fabric/)
- [Fabric IQ Overview](/fabric-iq-overview)
## Fine-tuning and Preference Optimization for Large Language Models on Azure
A hands-on guide is available for fine-tuning enterprise LLMs with Microsoft Foundry on Azure, taking models and aligning them for organization-specific requirements and policies. The documentation covers data prep, running training jobs, and benchmarking—applying methods to use cases like PubMed summarization for health and science.
Direct Preference Optimization (DPO) is also explained, detailing how human feedback can steer LLMs toward better outputs. DPO is now in the Foundry SDK, and tutorials include example code, best practices for parameter selection, and links to new documentation.
- [Beyond the Prompt – Why and How to Fine-tune Your Own Models](https://devblogs.microsoft.com/foundry/beyond-the-prompt-why-and-how-to-fine-tune-your-own-models/)
- [DPO Fine-Tuning Using Microsoft Foundry SDK](https://devblogs.microsoft.com/foundry/dpo-fine-tuning-using-microsoft-foundry-sdk/)',
    'This week’s ML coverage spotlights Microsoft Fabric’s expansion of analytics and machine learning, with practical routes for ML in production, fine-tuning workflows, and automated pipelines. Tools like Semantic Link and Foundry fine-tuning offer easier, AI-driven analytics and operational intelligence.',
    1771228800, 'ml', '/ml/roundups/weekly-ml-roundup-2026-02-16', 'TechHub',
    'TechHub', '3F90F458B809EFCCAB716AB7D0B0C804D15096A8C259D5C0E7C9F8F7DF5BAB6D', ',ML,Microsoft Fabric,Semantic Link,Power BI,OneLake,MLflow,Operational Analytics,Time Series Analytics,Kusto,Streaming Data,Anomaly Detection,LightGBM,SMOTE,Azure AI Foundry,LLM Fine Tuning,Direct Preference Optimization,Roundups,',
    false, false, false, false, false,
    true, false, 32
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
-- weekly-ml-roundup-2026-02-09
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ml-roundup-2026-02-09', 'roundups', 'Weekly Machine Learning Roundup: Spark Control and Real-Time Fabric',
    'This week’s machine learning news includes more advanced Spark pipeline management, improved real-time analytics, and visual upgrade options in Microsoft Fabric and Azure Synapse.
<!--excerpt_end-->
## Smarter Pipeline Orchestration in Shared Spark Environments
Admins of Spark workloads can now use a priority-based orchestration approach using job tags like Light/Critical, Medium/High, or Heavy/Best Effort with metadata for optimized job scheduling. This strategy, compatible with Microsoft Fabric and Synapse, supports both fixed and adaptive classification. Copilot-style agents monitor and adjust workload class, reducing human input and increasing stability. Ready-to-use sample notebooks and template tools are available to get started. These changes deepen pipeline control and protection, continuing last week’s surge management efforts.
- [Smart Pipelines Orchestration: Designing Predictable Data Platforms on Shared Spark](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/smart-pipelines-orchestration-designing-predictable-data/ba-p/4491766)
## Real-Time Analytics and ML in Spark Notebooks with Eventstreams Integration
Direct Eventstreams and Spark Notebook integration (preview) in Fabric means instant access to over 30 streaming data sources—like CDC databases or brokers—right from the Real-Time Hub. PySpark code is auto-generated, Entra ID secures access, and one-click imports enable fast prototyping and migration to production. Early community feedback is encouraged. This builds on earlier additions like "Get Data with Cloud Connection," supporting a smooth transition from batching to real-time analytics.
- [Integrating Real-Time Eventstreams with Spark Notebooks in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/bringing-together-fabric-real-time-intelligence-notebook-and-spark-structured-streaming-preview/)
## Improved Visualization for Real-Time Dashboards: Custom Series Colors
Microsoft Fabric’s dashboard series now features customizable data series colors for any chart, so teams can visually separate operational data for easier monitoring and clarity. Documentation covers usage, supporting ongoing dashboard improvements.
- [Introducing Data Series Colors: Enhanced Visualization Control in Real-Time Dashboards](https://blog.fabric.microsoft.com/en-US/blog/32713/)',
    'This week’s machine learning news includes more advanced Spark pipeline management, improved real-time analytics, and visual upgrade options in Microsoft Fabric and Azure Synapse.',
    1770624000, 'ml', '/ml/roundups/weekly-ml-roundup-2026-02-09', 'TechHub',
    'TechHub', 'DCE568F71193A1B053D892F5A00E8345EA0DBC84F16A29DDA4524375796AEF00', ',ML,Apache Spark,Spark Notebooks,Pipeline Orchestration,Job Scheduling,Workload Management,Microsoft Fabric,Azure Synapse,Real Time Analytics,Structured Streaming,Eventstreams,Streaming Data,Microsoft Entra ID,Dashboards,Data Visualization,Roundups,',
    false, false, false, false, false,
    true, false, 32
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
-- weekly-ml-roundup-2026-02-02
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ml-roundup-2026-02-02', 'roundups', 'Weekly Machine Learning Roundup: RL Diagnostics and Fabric Ops',
    'This week’s ML updates cover new ways to diagnose RL workflows, agent orchestration patterns, more structured evaluation methods, and enhancements for data engineering with Microsoft Fabric. Advancements are also noted for medical imaging and multimodal RL solutions.
<!--excerpt_end-->
## Reliability and Diagnostics in Production-Scale Reinforcement Learning
Microsoft engineering teams released guidance on troubleshooting RL agent instability in production. Traditional aggregate metrics often miss rare errors, so this week’s article presents slice-aware diagnostics to flag drift and instability at the per-token level (using log-ratio percentiles and CDF drift analysis). Agent “tail growth” signals increase risk and needs mitigation.
Open-source Post-Training Toolkit features include TRL integration, a CLI, and distributed monitoring for RL systems, enabling more detailed RL debugging in production.
- [Diagnosing Instability in Production-Scale Agent Reinforcement Learning](https://devblogs.microsoft.com/engineering-at-microsoft/diagnosing-instability-in-production-scale-agent-rl/)
## Local-First Agentic Automation and Multi-Agent Orchestration
Local-first, privacy-focused agent pipelines are growing in use. This week’s hands-on guide covers a podcast automation workflow using the Agent Framework, edge-based SLMs (Ollama), and local speech for on-premise orchestration. Modular design examples show search, review, and script generation using real-time observability with DevUI. Complete code and hardware tips are included.
- [Engineering a Local-First Agentic Podcast Studio: A Deep Dive into Multi-Agent Orchestration](https://techcommunity.microsoft.com/t5/microsoft-developer-community/engineering-a-local-first-agentic-podcast-studio-a-deep-dive/ba-p/4488947)
## Streamlined Model Evaluation and Selection with Microsoft Foundry
Model evaluation is now easier using Microsoft Foundry and GitHub Copilot. The step-by-step guide describes forming datasets, running repeatable benchmarks with metrics like F1/METEOR, and analyzing results using the Python SDK and Jupyter. Debugging and documentation guidance are provided, along with pointers to Foundry and Azure AI model leaderboard resources.
- [Evaluating AI Models with Microsoft Foundry and GitHub Copilot](/github-copilot/videos/evaluating-ai-models-with-microsoft-foundry-and-github-copilot)
## Data Engineering and Platform Operations with Microsoft Fabric
Fabric introduces workspace-level surge protection, minimizing the risk of resource spikes by limiting job concurrency and allowing exemptions for important workloads. This complements last week’s resource management changes for better cost control.
The new "Get Data with Cloud Connection" feature in Fabric Notebooks streamlines secure connections to cloud sources and provides code snippets, making developer workflows faster.
On-premises Data Gateway (January 2026 release) improves connectivity between CSV, Fabric, and Power BI, syncing with Power BI Desktop for easier data pipelines.
A new guide explains robust real-time pipelines in Fabric, including strategies for data validation, lag monitoring, network planning, logging, and clear ownership—all aimed at reducing pipeline downtime in complex environments.
- [Workspace-Level Surge Protection in Microsoft Fabric (Preview)](https://blog.fabric.microsoft.com/en-US/blog/surge-protection-gets-smarter-introducing-workspace-level-controls-preview/)
- [Fabric Connection inside Notebook (Preview)](https://blog.fabric.microsoft.com/en-US/blog/32822/)
- [On-premises Data Gateway January 2026 Release Overview](https://blog.fabric.microsoft.com/en-US/blog/on-premises-data-gateway-january-2026-release/)
- [Building a Reliable Real-Time Data Pipeline with Microsoft Fabric](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/building-a-reliable-real-time-data-pipeline-with-microsoft/ba-p/4489534)
## Multimodal Reinforcement Learning Advances in Medical Imaging
The GigaTIME project applies multimodal RL to radiology/pathology report writing, fusing text and image data for clearer, patient-specific documentation generation. Insights cover modeling, simulation, and automation, with actionable examples drawn from Microsoft’s latest research.
- [How AI is Learning to Write Smarter Medical Imaging Reports](https://www.microsoft.com/en-us/research/blog/unirg-scaling-medical-imaging-report-generation-with-multimodal-reinforcement-learning/)',
    'This week’s ML updates cover new ways to diagnose RL workflows, agent orchestration patterns, more structured evaluation methods, and enhancements for data engineering with Microsoft Fabric. Advancements are also noted for medical imaging and multimodal RL solutions.',
    1770019200, 'ml', '/ml/roundups/weekly-ml-roundup-2026-02-02', 'TechHub',
    'TechHub', 'D6906307202226495A19ED5942C7FA85C555C84FAB521BD3629250FAF053AAA5', ',ML,Reinforcement Learning,Rlhf,Production ML,Observability,Drift Detection,Post Training,Agent Orchestration,Multi Agent Systems,Local First,Model Evaluation,Microsoft Fabric,Data Engineering,Real Time Pipelines,Medical Imaging,Roundups,',
    false, false, false, false, false,
    true, false, 32
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
-- weekly-ml-roundup-2026-01-26
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ml-roundup-2026-01-26', 'roundups', 'Weekly Machine Learning Roundup: Fabric gains and Physical AI',
    'This week’s machine learning updates highlight improvements to developer toolkits, analytics, and applied AI for robotics. Microsoft’s ecosystem releases streamline development, testing, and deployment for a range of ML applications.
<!--excerpt_end-->
## Microsoft Fabric: Enhanced Data Engineering, Analytics, and Performance
Building on last week’s news about ML in Fabric, these updates provide enhanced security and speed for Spark workloads via Private Endpoints, cost-saving autoscale features, and up to 4x Spark performance improvements with the Native Execution Engine. The GigaOm report recognizes Fabric’s unified feature set and includes new controls for cost, scaled SQL pool management, and additional ML connectors. Serverless processing and new OneLake capabilities support flexible analytics and engineering. Real-Time Dashboards have further speed optimizations, boosting streaming and IoT analytics up to 6x or 10x faster. Updated documentation and ongoing events keep users informed.
- [Securely Scaling Spark Data Engineering in Microsoft Fabric](/ml/videos/securely-scaling-spark-data-engineering-in-microsoft-fabric)
- [Microsoft Fabric Data Warehouse: GigaOm Radar Leader and Outperformer](https://blog.fabric.microsoft.com/en-US/blog/microsoft-fabric-data-warehouse-named-a-leader-and-outperformer-in-gigaom-radar-for-data-warehouses/)
- [Performance Improvements for Microsoft Fabric Real-Time Dashboards](https://blog.fabric.microsoft.com/en-US/blog/faster-smoother-more-delightful-real-time-dashboards-performance-improvements/)
## Physical AI Advances: Microsoft Research’s Rho-alpha Robotics Model
Rho-alpha, from Microsoft Research, applies machine learning beyond data analytics by supporting physical robotics. Its underlying system combines natural language processing, multiple sensors, and controls, and supports continuous learning from user interactions. The platform aligns with earlier discussions around Copilot’s agentic updates and best-practice monitoring. Developers in robotics, manufacturing, and real-time control gain tools as APIs and SDKs are released, showing a unified approach similar to advances in Fabric and .NET AI.
- [Introducing Rho-alpha: Microsoft Research''s Robotics Model for Physical AI](https://www.linkedin.com/posts/satyanadella_introducing-rho-alpha-the-new-robotics-model-activity-7419757666660466688-jSpD)',
    'This week’s machine learning updates highlight improvements to developer toolkits, analytics, and applied AI for robotics. Microsoft’s ecosystem releases streamline development, testing, and deployment for a range of ML applications.',
    1769414400, 'ml', '/ml/roundups/weekly-ml-roundup-2026-01-26', 'TechHub',
    'TechHub', 'B8C5D1F9464443C39B95B3E263AC604B6593676FDE5801475DDDBA5D3C3E7664', ',ML,Microsoft Fabric,Data Engineering,Apache Spark,Native Execution Engine,Private Endpoints,Autoscale,OneLake,Serverless SQL,Real Time Dashboards,Streaming Analytics,IoT Analytics,Robotics,Physical AI,Microsoft Research,Roundups,',
    false, false, false, false, false,
    true, false, 32
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
-- weekly-ml-roundup-2026-01-19
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ml-roundup-2026-01-19', 'roundups', 'Weekly Machine Learning Roundup: Agents, HPC, and In-Database Search',
    'This week’s machine learning articles focus on efficient AI-driven workflows for industrial and research teams. There are updates on in-database semantic search, agent frameworks, industrial deployments, and research in the life sciences. Tutorials and case studies provide actionable examples and show practical adoption of advanced ML tools.
<!--excerpt_end-->
## Building AI Workflows with Microsoft Agent Framework and .NET AI Stack
Building on recent themes of local embeddings and agent-based architectures, Pamela Fox’s livestream series demonstrates using the Microsoft Agent Framework in Python, with coverage of RAG agent skills, modular and reproducible AI deployments, monitoring using OpenTelemetry, and orchestration via Magentic. Evaluation with Azure AI SDK rounds out the workflow.
.NET developers can join the AI Community Standup, which now features hands-on sessions using Semantic Kernel, AI Extensions, and orchestration tools—helping the .NET community move beyond chatbot projects to deeper AI integration.
- [Python + Agents: Livestream Series on Building AI Agents with Microsoft Agent Framework](https://techcommunity.microsoft.com/t5/microsoft-developer-community/join-our-free-livestream-series-on-building-agents-in-python/ba-p/4485731)
- [.NET AI Community Standup: Building AI Apps with the New .NET AI Stack](/ai/videos/net-ai-community-standup-building-ai-apps-with-the-new-net-ai-stack)
## Industrial ML and Scientific Workflows Powered by Azure HPC and Microsoft Discovery
New case studies highlight large-scale machine learning on Azure’s HPC resources, such as Neural Concept’s industrial engineering work with Azure GPUs and storage for AI training in automotive aerodynamics. Benchmarks show efficient model development that parallels what was seen last week in deep learning rollouts.
In drug discovery, Insilico Medicine’s Nach01 model deployed via Microsoft Discovery demonstrates secure, repeatable analytics in the life sciences, drawing on Azure ML’s compliance and deployment features.
- [Neural Concept Sets New Industrial AI Benchmark on Azure HPC for Automotive Aerodynamics](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/scaling-physics-based-digital-twins-neural-concept-on-azure/ba-p/4483403)
- [AI-Native Drug Discovery using Insilico Medicine’s Nach01 Model and Microsoft Discovery](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/ai-native-drug-discovery-using-insilico-medicine-s-nach01-model/ba-p/4484497)
## Expanding Vector Search in Databases: DiskANN in Azure SQL and Fabric SQL
DiskANN now enables large-scale, fast vector search directly inside Azure SQL and Fabric SQL, building on last week’s announcement of local-embedding in the Fabric Eventhouse. This lets teams implement semantic search, classification, and content analysis at the database level for less latency and stronger privacy, without relying on outside APIs.
- [DiskANN: Vector Indexing in Azure SQL and Fabric SQL Explained](/ai/videos/diskann-vector-indexing-in-azure-sql-and-fabric-sql-explained)',
    'This week’s machine learning articles focus on efficient AI-driven workflows for industrial and research teams. There are updates on in-database semantic search, agent frameworks, industrial deployments, and research in the life sciences. Tutorials and case studies provide actionable examples and show practical adoption of advanced ML tools.',
    1768809600, 'ml', '/ml/roundups/weekly-ml-roundup-2026-01-19', 'TechHub',
    'TechHub', 'FE7AF895D413FEBF3958F14C0C481E02BF2FEC8D53886E4594E9EAC0C22BA6CE', ',ML,AI Workflows,Agent Frameworks,Microsoft Agent Framework,.NET AI Stack,Semantic Kernel,Retrieval Augmented Generation,OpenTelemetry,Azure AI SDK,Azure HPC,GPU Training,Azure Machine Learning,Vector Search,DiskANN,Azure SQL,Microsoft Fabric SQL,Drug Discovery,Roundups,',
    false, false, false, false, false,
    true, false, 32
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
-- weekly-ml-roundup-2026-01-12
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ml-roundup-2026-01-12', 'roundups', 'Weekly Machine Learning Roundup: Local Embeddings and AV Cloud ML',
    'Machine learning updates focus on deploying AI securely and at scale. Microsoft adds new local embedding features for semantic search and retrieval augmented generation (RAG) in analytics and expands deep learning applications for autonomous vehicles through the cloud.
<!--excerpt_end-->
## Local Embedding Generation in Fabric Eventhouse
Microsoft enables text embedding creation in the Kusto Python sandbox within Fabric Eventhouse using Small Language Models (SLMs) such as jina-v2-small and e5-small-v2, via the slm_embeddings_fl() function.
Previously, developers needed Azure OpenAI endpoints for embeddings, which added dependency on remote APIs and could bring latency, cost, and privacy limitations. Now, local inference allows for lower overhead, reduced latency, and simpler compliance—improving scalability and automation for data processing teams.
Documentation provides step-by-step KQL and Python examples for embedding creation, real-time search, and automated processing, supporting efficient, secure AI adoption in Azure environments.
- [Create Embeddings in Fabric Eventhouse with Built-in Small Language Models (SLMs)](https://blog.fabric.microsoft.com/en-US/blog/create-embeddings-in-fabric-eventhouse-with-built-in-small-language-models-slms/)
## Deep Learning for Autonomous Vehicles on Azure
Wayve leverages Azure for distributed training and large-scale deployment of deep learning models in autonomous vehicles, extending advanced ML into connected mobility. Azure''s infrastructure supports big data handling and fast model rollout across GPU and TPU clusters, building on cloud-enabled AI operations for industrial applications.
- [AI that drives change: Wayve rewrites self-driving playbook with deep learning in Azure](https://news.microsoft.com/source/emea/features/ai-that-drives-change-wayve-rewrites-self-driving-playbook-with-deep-learning-in-azure/)',
    'Machine learning updates focus on deploying AI securely and at scale. Microsoft adds new local embedding features for semantic search and retrieval augmented generation (RAG) in analytics and expands deep learning applications for autonomous vehicles through the cloud.',
    1768204800, 'ml', '/ml/roundups/weekly-ml-roundup-2026-01-12', 'TechHub',
    'TechHub', '9191536E349A7DC43CACE0E7ABA3ECFC0B6A5864034D57CFDDA01929460E50B3', ',ML,Semantic Search,Retrieval Augmented Generation,Text Embeddings,Small Language Models,Microsoft Fabric,Eventhouse,Azure,Kusto,Python,Kql,Local Inference,Autonomous Vehicles,Deep Learning,Distributed Training,Roundups,',
    false, false, false, false, false,
    true, false, 32
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
-- weekly-ml-roundup-2025-12-22
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ml-roundup-2025-12-22', 'roundups', 'Weekly Machine Learning Roundup: Fabric Spark 4.0, Faster ML Ops',
    'Fabric Runtime 2.0 debuts with Apache Spark 4.0, while environment library management runs 2.5x faster and Python Spark sessions start 70% quicker.
<!--excerpt_end-->
## Microsoft Fabric ML Platform Advances
Fabric Runtime 2.0 (experimental) debuts with Apache Spark 4.0 for scalable distributed processing. Additional upgrades include Java 21, Scala 2.13, Python 3.12, and Delta Lake 4.0, aiding migration and analysis speed. The year-end review covers improvements in platform security, migration help, Copilot access, improved SQL/KQL tooling, and consistent DevOps support—summarizing a year centered on usability and developer needs.
- [Fabric Runtime 2.0 Experimental Preview: Scalable Data Engineering with Spark](https://blog.fabric.microsoft.com/en-US/blog/fabric-runtime-2-0-experimental-public-preview/)
- [Microsoft Fabric 2025 Recap: Unified Data and AI Innovations](https://blog.fabric.microsoft.com/en-US/blog/microsoft-fabric-2025-holiday-recap-unified-data-an-ai-innovation/)
## Performance, Reliability, and Security in ML Workflows
Fabric increases productivity with environment library management running up to 2.5 times faster for custom libraries, and Python Spark session startups now completing 70% quicker. New lightweight install modes are inbound for small deployments. Spark job orchestration supports Service Principal and Workspace Identity authentication, reducing reliance on user credentials in production pipelines. Updated documentation simplifies setup and migration.
- [Fabric Environment Library Management Performance Improvements for Developers](https://blog.fabric.microsoft.com/en-US/blog/fabric-environment-library-management-performance-improvement/)
- [Run Spark Job Definitions in Pipelines with Service Principal or Workspace Identity](https://blog.fabric.microsoft.com/en-US/blog/run-spark-job-definitions-in-pipelines-with-service-principal-or-workspace-identity/)
## Evaluation and Best Practices for Azure-based Document AI Pipelines
A practical guide outlines deploying and evaluating document AI workflows with Azure. The resource covers building a ground truth set, technical steps (OCR, labeling, retrieval), error assessment, and performance tuning with continuous monitoring. It includes architecture diagrams and code examples for developers working on enterprise IDP projects.
- [Evaluation Frameworks for Document Pipelines Using Azure AI & Search](https://techcommunity.microsoft.com/t5/azure-architecture-blog/from-large-semi-structured-docs-to-actionable-data-in-depth/ba-p/4474060)',
    'Fabric Runtime 2.0 debuts with Apache Spark 4.0, while environment library management runs 2.5x faster and Python Spark sessions start 70% quicker.',
    1766390400, 'ml', '/ml/roundups/weekly-ml-roundup-2025-12-22', 'TechHub',
    'TechHub', '689205B9A613C64FD8CFA8C2A8B315905D8B17EFB61A27DEA9C622B4CA09A4EE', ',ML,Microsoft Fabric,Fabric Runtime,Apache Spark,Spark 4.0,Delta Lake,Python 3.12,Java 21,Scala 2.13,MLOps,Authentication,Service Principal,Workspace Identity,Document AI,Azure AI Search,Roundups,',
    false, false, false, false, false,
    true, false, 32
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
-- weekly-ml-roundup-2025-12-15
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ml-roundup-2025-12-15', 'roundups', 'Weekly Machine Learning Roundup: Fabric, Agents, Biomed ML',
    'Updates in ML this week delivered more productive workflows and better data quality for large-scale and enterprise deployments. Microsoft improved ML engineering, pipeline automation, and operational tooling with Spark and agent frameworks.
<!--excerpt_end-->
## Microsoft Fabric Ecosystem: Engineering, Data Quality, and Automation
This week, Microsoft Fabric improvements target automation and barrier reduction in ML workflows. A step-by-step guide demonstrates how to automate data quality checks for every layer of a Medallion Architecture using Great Expectations for reusable, testable pipelines. The guide also explains how to integrate results with incident response and analytics workflows.
The new Forecasting Service allows for nearly instant Spark notebook startup, building on the recent focus on serverless infrastructure and cost efficiency. Articles this week explain dynamic scheduling and predictive scaling using Azure Cosmos DB and Data Explorer.
Variable Library is now available for Fabric Notebooks, offering centralized management for secrets and configuration, supporting automation and migration across environments.
Update to Fabric Real-Time Intelligence changes how Anomaly Detector is billed—from instance-based to query-based—helping teams monitor usage and control costs more effectively.
- [From Bronze to Gold: Data Quality Strategies for ETL in Microsoft Fabric](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/from-bronze-to-gold-data-quality-strategies-for-etl-in-microsoft/ba-p/4476303)
- [How Microsoft Fabric''s Forecasting Service Makes Spark Notebooks Instant](https://blog.fabric.microsoft.com/en-US/blog/how-fabric-makes-spark-notebook-feel-instant-proactive-resource-provisioning-for-scalable-data-science-data-engineering/)
- [Variable Library Support Now Available in Microsoft Fabric Notebooks](https://blog.fabric.microsoft.com/en-US/blog/variable-library-support-in-notebook-now-generally-available/)
- [Understanding Billing for Anomaly Detector in Microsoft Fabric’s Real-Time Intelligence](https://blog.fabric.microsoft.com/en-US/blog/billing-for-anomaly-detector-in-real-time-intelligence/)
## Reinforcement Learning in AI Agents: Agent Lightning Open Source Release
Microsoft Research Asia has open-sourced Agent Lightning, a framework designed for reinforcement learning (RL) with support for decoupled RL training and agent execution. The platform enables workflow optimization for existing LLMAgent frameworks, supports hierarchical RL for complex tasks, and allows flexible plug-in of new RL algorithms.
Agent Lightning streamlines logging, supports both GPU and CPU, and increases accuracy in a range of scenarios including text-to-SQL, RAG, and multi-agent QA. Continuous learning is planned. Development is underway to offer better prompt optimization and easy RL integration in live AI applications.
- [Agent Lightning: Making AI Agents Smarter Without Rewriting Code](https://www.microsoft.com/en-us/research/blog/agent-lightning-adding-reinforcement-learning-to-ai-agents-without-code-rewrites/)
## AI for Biomedical Workflows: GigaTIME Spatial Proteomics Platform
GigaTIME, a spatial proteomics platform, lets scientists use machine learning on digital slides to measure protein distributions at scale, removing the need for expensive assays. It supports broad analysis and rapid hypothesis generation for cancer research, representing practical ML for biomedical challenges.
- [AI-Powered Spatial Proteomics Platform GigaTIME Accelerates Cancer Discovery](https://www.linkedin.com/posts/satyanadella_ai-generated-population-scale-is-changing-activity-7404189540757831680-VtoO)',
    'Updates in ML this week delivered more productive workflows and better data quality for large-scale and enterprise deployments. Microsoft improved ML engineering, pipeline automation, and operational tooling with Spark and agent frameworks.',
    1765785600, 'ml', '/ml/roundups/weekly-ml-roundup-2025-12-15', 'TechHub',
    'TechHub', '6CF625F34B6ADD43DC68A8453E4EFB477FACCBC4FE7BD160029CFB6A6D143BE3', ',ML,Microsoft Fabric,Apache Spark,MLOps,Data Quality,Great Expectations,Medallion Architecture,Pipeline Automation,Forecasting,Serverless Computing,Anomaly Detection,Cost Management,Reinforcement Learning,AI Agents,Spatial Proteomics,Roundups,',
    false, false, false, false, false,
    true, false, 32
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
-- weekly-ml-roundup-2025-12-08
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ml-roundup-2025-12-08', 'roundups', 'Weekly Machine Learning Roundup: Serverless, Secure Pipelines, Segmentation',
    'Updates this week cover serverless workspaces, secure pipeline automation, and AI-enabled customer segmentation.
<!--excerpt_end-->
## Serverless Workspaces in Azure Databricks
Azure Databricks now supports public preview for Serverless workspaces, which remove the need for manual VNet and cluster setup. These changes support persistent data practices and are governed through Unity Catalog, with improvements in budget controls and security. Serverless egress and Private Link options increase compliance, with Python and SQL workflows now supported for scaling secure ML operations.
- [Serverless Workspaces Are Now Available in Azure Databricks](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/serverless-workspaces-are-live-in-azure-databricks/ba-p/4474712)
## Secure Automation for Notebooks in Fabric Data Factory Pipelines
Service Principal and Workspace Identity authentication are now available for running notebooks in Fabric Data Factory pipelines. This change reduces manual configuration, improves reliability, helps centralize identity management, and creates more robust production environments.
- [Run Notebooks in Pipelines with Service Principal or Workspace Identity](https://blog.fabric.microsoft.com/en-US/blog/run-notebooks-in-pipelines-with-service-principal-or-workspace-identity/)
## AI-Enabled Customer Segmentation Architecture
A joint case study from UCLA Anderson and Microsoft details a system for dynamic customer segmentation, helping B2B software businesses better handle resource allocation. The technical solution uses clustering, ML models like CatBoost and XGBoost, and an LLM assistant for workflow transparency. Azure handles orchestration and pipeline reliability for deployment.
- [KPI-Driven, AI-Enabled Tiering Architecture for Microsoft’s Global B2B Business](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/architecting-the-next-generation-customer-tiering-system/ba-p/4475326)',
    'Updates this week cover serverless workspaces, secure pipeline automation, and AI-enabled customer segmentation.',
    1765180800, 'ml', '/ml/roundups/weekly-ml-roundup-2025-12-08', 'TechHub',
    'TechHub', '4F74F8887FC2A9FD5064C8B032BD4E7E28646724DE3E479EB82D6F8DC4F10411', ',ML,Azure Databricks,Serverless,Unity Catalog,Azure Private Link,Data Egress Controls,Python,SQL,Microsoft Fabric,Fabric Data Factory,Notebook Pipelines,Service Principal,Workspace Identity,Customer Segmentation,Clustering,CatBoost,XGBoost,LLM Assistant,MLOps,Azure Orchestration,Roundups,',
    false, false, false, false, false,
    true, false, 32
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
-- weekly-ml-roundup-2025-12-01
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ml-roundup-2025-12-01', 'roundups', 'Weekly Machine Learning Roundup: Fine-Tuning, Databricks, Azure HPC',
    'At Ignite, machine learning updates centered on developer efficiency and fine-tuning at scale. This week’s coverage spotlights unified infrastructure, data platform improvements, and production agent guidance, all reinforcing the pattern toward integrated AI solutions, tuned models, and enterprise scaling within Azure.
<!--excerpt_end-->
## Microsoft Foundry and AI Agent Fine-Tuning
Furthering last week’s focus on custom workflows and model integration, Microsoft Foundry’s recent session covers all steps for producing and deploying tuned AI for real-world applications. This builds on Microsoft’s goal of making advanced ML techniques more widely available.
The session highlights Azure OpenAI and open-source models, with concrete examples using GPT-5 and O4 Mini. Synthetic data generation from Swagger specs also features heavily, supporting the need for robust training sets. Demos show how multiple agents collaborate to create, test, and improve synthetic data, increasing system reliability and business flexibility.
The ‘Navigator’ scenario illustrates how Foundry-powered agents process millions of contracts per day, underlining measurable benefits for both technical teams and leadership. Covered topics include model selection, API integration, and production deployment strategies, directly supporting earlier work in orchestration and ML.NET. For Azure or local teams, these guides bring ML workflows to greater maturity and scale.
- [AI Fine-Tuning in Microsoft Foundry: Building Production-Ready Agents](/ai/videos/ai-fine-tuning-in-microsoft-foundry-building-production-ready-agents)
## Azure Databricks: Unified Data and AI Ecosystem
Azure Databricks was featured as a unified analytics solution with extended integration in this week’s news. Tutorials cover new agent tools, such as Genie for rapid creation, Knowledge Assistant for management, and Multi-Agent Supervisor for routing—further supporting persistent workflow state and semantic data practices discussed previously.
The Databricks Connector, now improved for Power BI and Microsoft Apps, supports real-time data integration and workflow automation. The update to Databricks’ security tools—highlighted by Unity Catalog—matches the ongoing enterprise push for monitoring and compliance. Demonstrations, like EyeFi, reinforce Databricks’ expanding use in large organizations.
- [Accelerate Data and AI Transformation with Azure Databricks](/ai/videos/accelerate-data-and-ai-transformation-with-azure-databricks)
## Pushing the Boundaries: Azure AI Supercomputing Infrastructure
This week’s coverage dives into Azure’s updated supercomputing resources, with a focus on validating multi-billion parameter models using new GPU hardware (GB200/300, H100), advanced networking, and storage—building on past improvements in compute capacity.
Methodology guides for system inspection, performance tuning, and validation follow last week’s narrative around reliability and Azure’s blend of open source and built-in tooling. New GB300 GPUs expand capacity for growing models, and case studies (such as LLAMA and GRAC 314B) show Azure’s evolving capability for deployment and operations at scale.
- [Pushing Limits of Supercomputing Innovation on Azure AI Infrastructure](/ai/videos/pushing-limits-of-supercomputing-innovation-on-azure-ai-infrastructure)',
    'At Ignite, machine learning updates centered on developer efficiency and fine-tuning at scale. This week’s coverage spotlights unified infrastructure, data platform improvements, and production agent guidance, all reinforcing the pattern toward integrated AI solutions, tuned models, and enterprise scaling within Azure.',
    1764576000, 'ml', '/ml/roundups/weekly-ml-roundup-2025-12-01', 'TechHub',
    'TechHub', '90D0D45D61B1B002F6E2DFAD022FE1BE0DD8C8AA93BD24C9A95AF10485142413', ',ML,Azure AI,Microsoft Ignite,Microsoft Foundry,AI Agents,Fine Tuning,Azure OpenAI,Synthetic Data,Swagger,GPT 5,O4 Mini,Azure Databricks,Unity Catalog,Power BI,GPU Supercomputing,Roundups,',
    false, false, false, false, false,
    true, false, 32
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
-- weekly-ml-roundup-2025-11-24
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ml-roundup-2025-11-24', 'roundups', 'Weekly Machine Learning Roundup: Scalable Compute and ML Ops',
    'The machine learning focus this week is on more scalable compute, enhanced platform features, and better operational tools from cloud and enterprise providers. Azure rolled out ND GB300 v6 VMs, while Microsoft Fabric announced further improvements in its AI and data engineering offerings. Aspects like data quality, model deployment, and performance optimization remain front and center, reflecting an ongoing move to scalable and high-throughput ML infrastructure.
<!--excerpt_end-->
## Azure AI Compute and Infrastructure
Azure has released the ND GB300 v6 VMs, which include NVIDIA GB300 NVL72 GPUs, Grace CPUs, and fast InfiniBand networking built for large-scale training and inference. These VMs integrate with Azure CycleCloud, Batch, and AKS, building on existing solutions for orchestrating AI workloads.
The AMLFS 20 (Azure Managed Lustre) SKU delivers bigger namespaces and higher metadata throughput for high-performance workloads, meeting the needs of fast, scalable data access in ML production.
- [Azure ND GB300 v6 Virtual Machines: General Availability and Next-Gen AI Infrastructure](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/azure-nd-gb300-v6-now-generally-available-hyper-optimized-for/ba-p/4469475)
- [Announcing Public Preview of AMLFS 20: New Azure Managed Lustre SKU for AI and HPC](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/announcing-the-public-preview-of-amlfs-20-azure-managed-lustre/ba-p/4470665)
## Model Development, Deployment, and Optimization Tools
Microsoft Foundry and Azure ML are focusing on seamless model development and production deployment, helping teams standardize their ML pipelines and cover scenarios like reinforcement learning and intelligent agent deployment. Sessions and tutorials explore metric evaluation, reliability testing, and parameter tuning for Retrieval-Augmented Generation (RAG) agents.
Windows ML updates show ongoing work to enable local AI inference using ONNX Runtime, supporting privacy and low-latency requirements, following previous guidance for regulated environments.
- [Training and Deploying Reasoning Models with Microsoft Foundry and Azure ML](/ai/videos/training-and-deploying-reasoning-models-with-microsoft-foundry-and-azure-ml)
- [Debugging and Optimizing RAG Agents in Microsoft Foundry](https://devblogs.microsoft.com/foundry/how-to-debug-and-optimize-rag-agents-in-azure-ai-foundry/)
- [Deploying Local AI Models in Enterprise with Windows ML](/ai/videos/deploying-local-ai-models-in-enterprise-with-windows-ml)
## Microsoft Fabric: Enhanced AI and Data Engineering Capabilities
Microsoft Fabric’s latest updates provide more flexible AI integration, with features like ai.embed() (now GA) and support for models from GPT-5, Claude, LLaMA, Azure OpenAI, and AI Foundry. These tools bring AI-powered workflows into common data engineering platforms, facilitating new uses for PySpark, pandas, and hybrid agent workflows.
Updates for event streaming, data clustering, and endpoint management make it easier to unify analytics workloads and speed up real-time processing with KQL/SQL support. dbt Jobs integration expands on recent improvements to data transformation and validation in Fabric.
- [Microsoft Fabric AI Functions: Enhanced Features Now Generally Available](https://blog.fabric.microsoft.com/en-US/blog/29826/)
- [Eventhouse Endpoint Arrives for Microsoft Fabric Data Warehouse](https://blog.fabric.microsoft.com/en-US/blog/introducing-eventhouse-endpoint-for-fabric-data-warehouse-real-time-analytics-unified-architecture/)
- [Announcing Data Clustering in Microsoft Fabric Data Warehouse](https://blog.fabric.microsoft.com/en-US/blog/announcing-data-clustering-in-fabric-data-warehouse-preview/)
- [Integrating dbt Jobs with Microsoft Fabric for Scalable SQL Transformations (Preview)](https://blog.fabric.microsoft.com/en-US/blog/dbt-job-in-microsoft-fabric-ship-trustworthy-sql-models-faster-preview/)
## Data Quality, Analytics, and Platform Integration
Following up on historical dataset modernization, this week’s content provides more strategies for proactive data quality management, supporting cleaner ML pipelines for any cloud setup.
Further coverage shows Azure Databricks and SAP Business Data Cloud links for modern analytics, with stories about Delta Sharing, agent-based automation, and Power BI integrations that help connect disparate data sources and expand AI development.
- [Continuous Data Quality Optimization for Better AI Output](https://dellenny.com/continuous-data-quality-optimization-for-ai-the-essential-guide/)
- [Modern Data Analytics and AI with SAP Databricks on Azure](/ai/videos/modern-data-analytics-and-ai-with-sap-databricks-on-azure)',
    'The machine learning focus this week is on more scalable compute, enhanced platform features, and better operational tools from cloud and enterprise providers. Azure rolled out ND GB300 v6 VMs, while Microsoft Fabric announced further improvements in its AI and data engineering offerings. Aspects like data quality, model deployment, and performance optimization remain front and center, reflecting an ongoing move to scalable and high-throughput ML infrastructure.',
    1763971200, 'ml', '/ml/roundups/weekly-ml-roundup-2025-11-24', 'TechHub',
    'TechHub', '3CE953B73ACDD0EF7FF2A2DE23AB9DC37C4D583E362853351F83C5E609006F0A', ',Azure,Azure AI,Azure Machine Learning,Azure ND GB300 V6,NVIDIA GB300 NVL72,InfiniBand,Azure Managed Lustre,AMLFS 20,AKS,Azure Batch,Azure CycleCloud,Microsoft Fabric,RAG,ONNX Runtime,Dbt,ML,Roundups,',
    false, false, false, false, false,
    true, false, 32
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
-- weekly-ml-roundup-2025-11-17
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ml-roundup-2025-11-17', 'roundups', 'Weekly Machine Learning Roundup: Open Models and Agent Patterns',
    'Machine learning highlights this week include new open-source models, practical cloud integration examples, and inventive applications for research and cultural projects. Microsoft furthered its open climate research and shared patterns for agent workflows and legacy dataset modernization.
<!--excerpt_end-->
## Microsoft’s Open-Source Aurora Model for Climate Forecasting
Microsoft debuted the Aurora project to expand access to climate and weather modeling—an open-source foundation trained on broad atmospheric datasets for predicting waves, air quality, and extreme weather. Code, model weights, and pipeline plans are available, making it easier for developers to offer both localized and large-scale forecasts. Built through partnerships including Cambridge’s Rich Turner lab and built atop efforts like SPARROW, Aurora’s public APIs make it a useful resource for energy management, disaster response, and environmental analysis by reducing the technical hurdles for entry.
- [Aurora: Microsoft’s Open-Source AI Model for Weather and Climate Forecasting](https://blogs.microsoft.com/on-the-issues/2025/11/13/the-next-phase-of-aurora-open-and-collaborative-ai-for-weather-and-climate-forecasting/)
## .NET, Aspire, and Redis: Patterns for Intelligent Agentic Workflows
Detailed coverage of .NET Aspire, Redis, and the Microsoft Agent Framework shows how to build robust, scalable agent systems. Redis enables semantic caching, vector storage, and management of session state, aligning with the trend toward persistent, distributed agent architectures. All updates utilize the new features in .NET 10, C# 14, F# 10, and Visual Studio 2026, reinforcing the focus on modular and multi-agent workflow strategies.
- [Build Smarter Agents with Redis, .NET Aspire, and Microsoft Agent Framework](/ai/videos/build-smarter-agents-with-redis-net-aspire-and-microsoft-agent-framework)
## Modernizing Historical Datasets with ML.NET and Azure
ML.NET and Azure CosmosDb are used this week to modernize a 17th-century Italian-English dictionary. Developers leverage current .NET and ML.NET features for processing legacy data—including custom vector embeddings and scalable cloud storage. These updates enable robust semantic search and reliable API endpoints, demonstrating practical uses of Microsoft’s ML tools in both research and preservation settings.
- [Modernizing a 17th Century Italian-English Dictionary with .NET and ML.NET](/azure/videos/modernizing-a-17th-century-italian-english-dictionary-with-net-and-mlnet)',
    'Machine learning highlights this week include new open-source models, practical cloud integration examples, and inventive applications for research and cultural projects. Microsoft furthered its open climate research and shared patterns for agent workflows and legacy dataset modernization.',
    1763366400, 'ml', '/ml/roundups/weekly-ml-roundup-2025-11-17', 'TechHub',
    'TechHub', 'BA82908D5B2AADA23AA638D70037A24FC486F6E572888FDDD19F445294F5AA04', ',ML,Open Source,Foundation Models,Climate Modeling,Weather Forecasting,Environmental Data,Agentic Workflows,Multi Agent Systems,Microsoft Agent Framework,.NET Aspire,Redis,Vector Embeddings,Semantic Search,ML.NET,Azure Cosmos DB,Roundups,',
    false, false, false, false, false,
    true, false, 32
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
-- weekly-ml-roundup-2025-11-10
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ml-roundup-2025-11-10', 'roundups', 'Weekly Machine Learning Roundup: Faster Inference and Scalable AI',
    'Recent advancements in machine learning include new hardware performance benchmarks, updates to distributed computing platforms, practical AI workflow guides, improvements in geospatial analytics tools, and the introduction of a new open-source platform for agent-based market simulation. These updates provide concrete help for teams deploying large-scale ML and modernizing practices.
<!--excerpt_end-->
## Azure ML Infrastructure and Hardware Optimization
Azure''s ND GB300 v6 virtual machines, equipped with Blackwell GPUs, achieved over 1 million tokens/sec on Llama2 70B inference, surpassing the performance of previous ND GB200 v6 and DGX H100 models. Technical documentation outlines stack improvements such as 2.5x GEMM TFLOPS, 7.37TB/s bandwidth, and multi-VM orchestration, offering reproducible benchmarking scripts and advice for optimizing large language model (LLM) inference on Azure.
- [Azure ND GB300 v6: Achieving Over 1 Million Tokens/sec on Llama2 70B Inference](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/breaking-the-million-token-barrier-the-technical-achievement-of/ba/p/4466080)
## Distributed Python AI with Ray on Azure
Microsoft and Anyscale introduced managed Ray support on Azure Kubernetes Service, featuring Azure Monitor, Entra ID, and Blob Storage integration. Python developers can now deploy distributed ML tasks securely and scale resources easily, without deep Kubernetes expertise. Key features include RayTurbo, simple cluster deployment, and compliance/security within customer subscriptions—streamlining the path from prototype to production.
- [Powering Distributed AI and ML Workloads at Scale with Azure and Anyscale](https://devblogs.microsoft.com/all-things-azure/powering-distributed-aiml-at-scale-with-azure-and-anyscale/)
## Practical AI Workflows: Tutorials and Educational Initiatives
The Spanish-language ''Python + IA'' series offers nine practical sessions on building and deploying GenAI apps, addressing LLMs, RAG, agent engineering, and risk mitigation with code samples and community support on Azure and GitHub. The Cozy Kitchen guide demonstrates intelligent agent engineering with Azure AI Foundry, focusing on modular workflow design, persistence, GitHub integration, and advanced tuning.
- [Recapitulación de la Serie Python + IA: Técnicas, Modelos y Recursos](https://techcommunity.microsoft.com/t5/microsoft-developer-community/python-ia-resumen-y-recursos/ba-p/4465152)
- [From Building to Fine-Tuning: Coding Agents that Optimize AI Workflows](/ai/videos/from-building-to-fine-tuning-coding-agents-that-optimize-ai-workflows)
## Microsoft Fabric Data Services: Spatial Analytics, Workflow Automation, and Data Skills Development
ArcGIS GeoAnalytics is generally available for Fabric Spark users, enabling robust spatial data automation and visualization. Fabric Data Days, a global workshop event, now provides training and competitions for data engineers and scientists. Updates to Fabric introduce decoupled semantic models and API-driven workflow management, improving model lifecycle flexibility.
- [ArcGIS GeoAnalytics for Microsoft Fabric Spark (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/arcgis-geoanalytics-for-microsoft-fabric-spark-generally-available/)
- [Advance your career in Data & AI with Microsoft Fabric Data Days](https://blog.fabric.microsoft.com/en-US/blog/advance-your-career-in-data-ai-with-microsoft-fabric-data-days/)
- [Decoupling Default Semantic Models for Existing Items in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/decoupling-default-semantic-models-for-existing-in-microsoft-fabric/)
## Open-Source Platforms and Agent-Based Market Simulation
Microsoft’s open-source Magentic Marketplace provides a modular system for agent-based market simulation. It includes REST APIs, customizable agent and market primitives, visualizations, and research summaries. Resources such as source code, datasets, and experiment templates are available for developers and researchers to study transparency and resilience in agent-based systems.
- [Magentic Marketplace: Open-Source Simulation for Agentic Markets Research](https://www.microsoft.com/en-us/research/blog/magentic-marketplace-an-open-source-simulation-environment-for-studying-agentic-markets/)',
    'Recent advancements in machine learning include new hardware performance benchmarks, updates to distributed computing platforms, practical AI workflow guides, improvements in geospatial analytics tools, and the introduction of a new open-source platform for agent-based market simulation. These updates provide concrete help for teams deploying large-scale ML and modernizing practices.',
    1762761600, 'ml', '/ml/roundups/weekly-ml-roundup-2025-11-10', 'TechHub',
    'TechHub', 'AB0051388439D00D144A7B8935823F2D3ADF4AE1C9C5641EDB2772E2027EC7FA', ',ML,Azure,Azure Machine Learning,GPU Inference,NVIDIA Blackwell,Llama 2,LLM Optimization,Benchmarking,Ray,AKS,Distributed Computing,Microsoft Fabric,Geospatial Analytics,Agent Based Simulation,Roundups,',
    false, false, false, false, false,
    true, false, 32
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
-- weekly-ml-roundup-2025-11-03
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ml-roundup-2025-11-03', 'roundups', 'Weekly Machine Learning Roundup: Vector Search Lands in SQL',
    'In ML news this week, attention shifted to practical integration of vector search features into enterprise databases. This continues the trend of enabling smarter semantic search and similarity-based retrieval within everyday database operations, building on recent progress in GPT-4o fine-tuning and retrieval pipelines for Azure AI Foundry.
<!--excerpt_end-->
## Vector Search in SQL Server 2025 and Azure SQL
SQL Server 2025 now offers built-in vector search, highlighted in the Data Exposed: MVP Edition walkthrough led by Joseph D’Antoni. The guide demonstrates setting up vector storage, retrieval, and using new Transact-SQL functions for semantic and similarity search, supporting a range of AI workloads.
Resources include setup walkthroughs, query examples, and architectural details for Azure SQL, continuing the availability of public samples and deployment tools for vector-aware development.
- [Implementing Vector Search in Your Application with SQL Server 2025](/ai/videos/implementing-vector-search-in-your-application-with-sql-server-2025)',
    'In ML news this week, attention shifted to practical integration of vector search features into enterprise databases. This continues the trend of enabling smarter semantic search and similarity-based retrieval within everyday database operations, building on recent progress in GPT-4o fine-tuning and retrieval pipelines for Azure AI Foundry.',
    1762156800, 'ml', '/ml/roundups/weekly-ml-roundup-2025-11-03', 'TechHub',
    'TechHub', '01DC1B81E58139CE5EA68E71B452F071E14774AF21C6FB296B1C3FBE31E1EB68', ',ML,Vector Search,SQL Server 2025,Azure SQL,Embeddings,Semantic Search,Similarity Search,Transact SQL,Retrieval Augmented Generation,RAG,Azure AI Foundry,GPT 4o Fine Tuning,Enterprise Databases,Database Architecture,Deployment Tools,Roundups,',
    false, false, false, false, false,
    true, false, 32
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
-- weekly-ml-roundup-2025-10-27
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ml-roundup-2025-10-27', 'roundups', 'Weekly Machine Learning Roundup: VLM Fine-Tuning on Azure',
    'Recent progress in machine learning underlines improvements in model fine-tuning and deployment, emphasizing vision-language models (VLMs) for image classification on Azure AI Foundry. Developers are given clearer steps for achieving better accuracy, controlling costs, and supporting production deployments.
<!--excerpt_end-->
## Fine-Tuning GPT-4o Vision-Language Models on Azure AI Foundry
A new guide covers fine-tuning GPT-4o for image classification (using Stanford Dogs), continuing last week’s push for performance and usability in ML stacks. The tutorial covers data formatting in Azure JSONL, using Batch Inference API for large workloads (with higher latency and reduced cost), and connects to past automation topics drawn from Microsoft Fabric.
Instructions include using the Vision Fine-Tuning API to adapt GPT-4o for breed identification. The inclusion of public code samples and templates supports research and encourages wider use, echoing Azure ML’s focus on analytics and efficiency. Demonstrated results improved accuracy from 61.67% (CNN) to 82.67% for a fine-tuned model, with a detailed breakdown of cost and latency to help with deployment planning.
Production guidance centers around Azure’s security and scalability, detailing parameter adjustment, throughput, and best practices. Open-source code and Azure documentation make this a practical resource for ML engineers.
- [Fine-Tuning GPT-4o for Image Classification on Azure AI Foundry: A Practical Guide](https://devblogs.microsoft.com/foundry/a-developers-guide-to-fine-tuning-gpt-4o-for-image-classification-on-azure-ai-foundry/)',
    'Recent progress in machine learning underlines improvements in model fine-tuning and deployment, emphasizing vision-language models (VLMs) for image classification on Azure AI Foundry. Developers are given clearer steps for achieving better accuracy, controlling costs, and supporting production deployments.',
    1761552000, 'ml', '/ml/roundups/weekly-ml-roundup-2025-10-27', 'TechHub',
    'TechHub', '2AA8935F76AC1A70327FFF806AD628706F87ECC5C4FEBF392DFBE0C2736815F6', ',ML,Vision Language Models,GPT 4o,Image Classification,Model Fine Tuning,Azure AI Foundry,Vision Fine Tuning API,Batch Inference API,Azure Machine Learning,JSONL,Stanford Dogs Dataset,MLOps,Cost Optimization,Latency,Production Deployment,Roundups,',
    false, false, false, false, false,
    true, false, 32
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
-- weekly-ml-roundup-2025-10-20
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ml-roundup-2025-10-20', 'roundups', 'Weekly Machine Learning Roundup: Faster Lakehouse Workflows',
    'Recent ML updates target smoother data engineering and greater Azure integration, making performance and reliability improvements for lakehouse and machine learning frameworks common in big data workflows.
<!--excerpt_end-->
## Microsoft Fabric Spark: Adaptive File Size Management for Delta Tables
Fabric Spark introduces adaptive file size management, automatically choosing optimal Delta table file sizes based on telemetry data. This automation streamlines ELT and analytics tasks, resulting in up to 2.8 times faster file compaction and 1.6 times TPC-DS performance improvements. Settings update automatically as workloads shift, but developers can tailor configurations to suit specific needs.
Benefits also include improved data skipping, reduced file rewrite costs, and increased processing parallelism, all supporting secure and flexible solutions.
- [Adaptive Target File Size Management in Fabric Spark](https://blog.fabric.microsoft.com/en-US/blog/adaptive-target-file-size-management-in-fabric-spark/)
## Azure Data Lake Integrations: adlfs Python Library Improvements
The adlfs Python library receives speed improvements through parallel block uploads and smaller chunk defaults, helping users avoid timeouts on geo-distributed systems and supporting more secure data pipelines.
Frameworks like Dask, Pandas, Ray, PyTorch, and PyIceberg work seamlessly with these updates, which include easier authentication and continued fsspec compatibility, supporting efficient integration for modern data and AI workflows.
- [Easily Connect AI Workloads to Azure Blob Storage with adlfs](https://devblogs.microsoft.com/azure-sdk/easily-connect-ai-workloads-to-azure-blob-storage-with-adlfs/)',
    'Recent ML updates target smoother data engineering and greater Azure integration, making performance and reliability improvements for lakehouse and machine learning frameworks common in big data workflows.',
    1760943600, 'ml', '/ml/roundups/weekly-ml-roundup-2025-10-20', 'TechHub',
    'TechHub', '5FD0DA6290AC2E693727121372D9975933D53CD84B4BA25819FBF776ED00ACB3', ',ML,Data Engineering,Azure,Microsoft Fabric,Apache Spark,Delta Lake,Lakehouse,Elt,Analytics,Azure Data Lake,Azure Blob Storage,Adlfs,Python,Fsspec,Dask,Roundups,',
    false, false, false, false, false,
    true, false, 32
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
-- weekly-ml-roundup-2025-10-13
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ml-roundup-2025-10-13', 'roundups', 'Weekly Machine Learning Roundup: Fabric, Edge AI, and Automation',
    'Machine learning updates this week focused on streamlined data engineering, automation, security, and developer skill-building resources across platforms. Microsoft Fabric’s toolset expanded Spark and SQL integrations. New documentation and security improvements, connectors, and educational material help developers working on analytics, edge AI, and automated data workflows.
<!--excerpt_end-->
## Microsoft Fabric Ecosystem: Streamlined Data Connectivity and Automation
Building on last week’s Dataflow Gen2 governance, Microsoft Fabric offers a Spark Connector for SQL Databases in preview, improving Spark workloads with direct access to Azure SQL, Managed Instance, and Fabric SQL. This simplifies ETL and ML for PySpark and Scala, continuing support for secure, enterprise standards.
OPENROWSET now lets users set named sources and relative file paths, replacing GUIDs for clear SQL and easier troubleshooting, furthering recent operational efficiency.
Service Principal support in Semantic Link enables scalable, secure automation of pipelines—continuing previous enhancements in permission and identity management. Azure AD managed identities and Key Vaults support role-based data jobs through the "sempy.fabric" package.
- [Fabric Spark Connector for SQL Databases Now Available (Preview)](https://blog.fabric.microsoft.com/en-US/blog/spark-connector-for-sql-databases-preview/)
- [Service Principal Support in Semantic Link: Enabling Scalable, Secure Automation](https://blog.fabric.microsoft.com/en-US/blog/service-principal-support-in-semantic-link-enabling-scalable-secure-automation/)
- [Simplifying File Access in OPENROWSET: Data Sources and Relative Paths (Preview)](https://blog.fabric.microsoft.com/en-US/blog/simplifying-file-access-in-openrowset-using-data-sources-and-relative-paths-preview/)
## Learning and Developer Enablement: Gen AI and Edge AI Resources
Expanded developer support includes a nine-part YouTube series from Pamela Fox, covering generative AI, prompt engineering, RAG, agent frameworks, and live code demonstrations with OpenAI SDK and Azure AI Search—following last week’s collaborative engagement and analytics in Fabric ML.
A new edge AI curriculum covers Windows AI PC and hardware deployment with ONNX Runtime, DirectML, and Olive, advancing last week’s hybrid architecture support. Practical samples address IoT and automation scenarios for NPUs and Azure connections.
- [Level Up Your Python Gen AI Skills: Nine-Part YouTube Stream Series](https://techcommunity.microsoft.com/t5/microsoft-developer-community/level-up-your-python-gen-ai-skills-from-our-free-nine-part/ba-p/4459464)
- [Building Smarter Edge AI with Windows AI PCs: The Edge AI for Beginners Curriculum](https://techcommunity.microsoft.com/t5/microsoft-developer-community/from-cloud-to-chip-building-smarter-ai-at-the-edge-with-windows/ba/p/4459582)
## The Emergence of Automated Data Modeling and Warehouse Modernization
Flow.BI’s AI-powered data modeling adopts metadata-driven automation, supporting model generation, relationship inference, multilingual metadata, mesh configuration, and robust security. This continues last week’s focus on metadata management, helping modernize data warehousing for organizations adapting their architecture.
- [Defining the Raw Data Vault with Artificial Intelligence: Automating Data Warehouse Modeling](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/defining-the-raw-data-vault-with-artificial-intelligence/ba-p/4453557)',
    'Machine learning updates this week focused on streamlined data engineering, automation, security, and developer skill-building resources across platforms. Microsoft Fabric’s toolset expanded Spark and SQL integrations. New documentation and security improvements, connectors, and educational material help developers working on analytics, edge AI, and automated data workflows.',
    1760338800, 'ml', '/ml/roundups/weekly-ml-roundup-2025-10-13', 'TechHub',
    'TechHub', '6A03257F5A3770CB5D2CD682DBD5A9D673AAE3AFBB1DBDFB39C6707754F791E9', ',ML,Microsoft Fabric,Apache Spark,PySpark,Scala,Azure SQL,SQL,ETL,Data Engineering,OPENROWSET,Service Principals,Managed Identities,ONNX Runtime,DirectML,RAG,Roundups,',
    false, false, false, false, false,
    true, false, 32
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
-- weekly-ml-roundup-2025-10-06
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ml-roundup-2025-10-06', 'roundups', 'Weekly Machine Learning Roundup: Fabric ETL, AI Data Prep, Experiments',
    'This week’s updates in machine learning focus on deeper integrations with the Microsoft Fabric ecosystem: faster, more affordable data transformations, enhanced multitasking, and streamlined collaboration. Broader connectivity between cloud and analytics platforms and stronger tools for experimentation mark a focus on flexible, interactive enterprise workflows.
<!--excerpt_end-->
## Dataflow Gen2 in Microsoft Fabric: Performance, Integration, and Developer Experience
Dataflow Gen2 in Microsoft Fabric gains a new pricing model, helping organizations manage ETL costs for jobs of all sizes. The Modern Query Evaluation Service speeds up parallel queries for lower expenses and shorter runtimes, advancing last week’s troubleshooting features like Spark monitoring APIs.
Real-time analytics and previews allow faster iteration on transformation logic, with outputs now targeting Fabric Lakehouse, Azure Data Lake Gen2, SharePoint (CSV), Snowflake (preview), and OneLake Catalog management—matching the trend of multi-environment integration.
Copilot now enables natural language transformation and ingestion, contributing to collaborative machine learning themes. Migration from Gen1 is supported by dedicated tools. Permission management, schema controls, and hybrid architecture improvements continue the previous focus on operational governance.
- [Announcing Dataflow Gen2 Enhancements in Microsoft Fabric: Faster, Smarter, and More Affordable Data Transformations](https://blog.fabric.microsoft.com/en-US/blog/unlocking-the-next-generation-of-data-transformations-with-dataflow-gen2-fabcon-europe-2025-announcements/)
- [Enhancements to Dataflow Gen2 in Microsoft Fabric: Expanded Destinations and Schema Support](https://blog.fabric.microsoft.com/en-US/blog/new-dataflow-gen2-data-destinations-and-experience-improvements/)
## AI-Powered Data Transformation and Developer Tools
Fabric Data Wrangler now supports fast AI-driven text summarization, translation, and sentiment analysis through PROSE suggestions and live previews. Copilot prompts generate custom transformation code and feedback, minimizing manual coding for complex datasets. Conversion between pandas and PySpark further scales projects, while documentation and guides support adoption of these new workflows.
- [Accelerate Data Transformation with AI in Data Wrangler](https://blog.fabric.microsoft.com/en-US/blog/accelerate-data-transformation-with-ai-functions-in-data-wrangler/)
## Multitasking and Workflow Improvements in Microsoft Fabric
Fabric’s updated horizontal tabs permit working on multiple items, along with workspace color coding and numbering to prevent errors and reduce context switching. The Object Explorer and higher concurrent item limits cater to users who need advanced multitasking—building on recent improvements for async processing and VS Code extension integration. These features are specific to Fabric.
- [Supercharge Your Workflow: New Multitasking Features Coming to Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/supercharge-your-workflow-new-multitasking-features-coming-to-fabric/)
## Experimentation Analytics with Statsig in Microsoft Fabric
Statsig Experimentation Analytics in Fabric provides tools for running and analyzing A/B/n tests on OneLake data, using frequentist statistics and near real-time metrics via Statsig’s Explorer. Instant results allow rapid update cycles, and Power BI integration assists visual review of experiments. Structured workflows help teams validate ML models, continuing last week’s focus on practical MLOps processes.
- [Experimentation Analytics with Statsig in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/27219/)',
    'This week’s updates in machine learning focus on deeper integrations with the Microsoft Fabric ecosystem: faster, more affordable data transformations, enhanced multitasking, and streamlined collaboration. Broader connectivity between cloud and analytics platforms and stronger tools for experimentation mark a focus on flexible, interactive enterprise workflows.',
    1759734000, 'ml', '/ml/roundups/weekly-ml-roundup-2025-10-06', 'TechHub',
    'TechHub', '8AE9251F1DE3DEBAC3446B60E912CF81843B430E0108623011DB12261CA7C516', ',ML,Microsoft Fabric,Dataflow Gen2,ETL,Modern Query Evaluation Service,OneLake,Lakehouse,Azure Data Lake Storage Gen2,Snowflake,SharePoint,Copilot,Data Wrangler,PySpark,Pandas,A/B Testing,Statsig,Power BI,MLOps,Data Governance,Roundups,',
    false, false, false, false, false,
    true, false, 32
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
-- weekly-ml-roundup-2025-09-29
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ml-roundup-2025-09-29', 'roundups', 'Weekly Machine Learning Roundup: Fabric Spark Observability and MLOps',
    'Machine learning updates this week focus on analytics scale, architecture maturity, and observability—especially in Microsoft Fabric’s Spark environment. New diagnostics and APIs offer developers more control, with an ongoing emphasis on collaborative production ML and best operational practices.
<!--excerpt_end-->
## Microsoft Fabric Spark Observability and Integration
A new preview for Fabric Spark Applications Comparison lets users visually assess up to four Spark app runs, supporting easier identification of performance issues. This builds on Spark Run Series Analysis, now generally available for grouping job runs and finding anomalies. Monitoring APIs provide real-time insight and automation capabilities for scaling ML operations. Features like Spark Advisor, skew diagnostics, and allocation reporting strengthen automated observability for teams.
User Data Functions, now generally available, enable custom Python logic in Fabric SQL, Lakehouse, Warehouses, and Power BI, encouraging wider reuse and easier integration. The VS Code extension and async data processing further improve developer workflow.
- [Microsoft Fabric Spark Applications Comparison Feature (Preview)](https://blog.fabric.microsoft.com/en-US/blog/public-preview-announcement-fabric-spark-applications-comparison/)
- [Fabric Spark Run Series Analysis: Enterprise-Scale Observability for Microsoft Fabric Spark Jobs](https://blog.fabric.microsoft.com/en-US/blog/fabric-spark-run-series-analysis-generally-available/)
- [Fabric Spark Monitoring APIs Now Generally Available](https://blog.fabric.microsoft.com/en-US/blog/general-availability-announcement-fabric-spark-monitoring-apis/)
- [Fabric User Data Functions Now Generally Available](https://blog.fabric.microsoft.com/en-US/blog/announcing-fabric-user-data-functions-now-in-general-availability/)
## Evolving MLOps Architectures and Operational Practices
Ongoing best practices encourage the shift from ad-hoc ML deployment to modular, automated workflows with versioning, CI/CD, lifecycle management, and monitoring—with tools like Kafka, Spark Streaming, Feast, MLflow, and Kubernetes as central components. The focus is on continuous delivery, drift detection, and strong governance within practical ML lifecycle management.
Community discussions around MLOps support collaborative learning, with events, podcasts, and networking driving shared expertise in real-world deployment, governance, and technical debt management.
- [MLOps Architectures: Building Scalable AI Systems](https://dellenny.com/mlops-architectures-building-scalable-ai-systems/)
- [MLOps at Scale: How Community Is Driving AI Into Production](https://devops.com/mlops-at-scale-how-community-is-driving-ai-into-production/)',
    'Machine learning updates this week focus on analytics scale, architecture maturity, and observability—especially in Microsoft Fabric’s Spark environment. New diagnostics and APIs offer developers more control, with an ongoing emphasis on collaborative production ML and best operational practices.',
    1759129200, 'ml', '/ml/roundups/weekly-ml-roundup-2025-09-29', 'TechHub',
    'TechHub', '9790B5A1B931EF497BC62F5F62CE3951BF28D728D45C9F29DA15DFF59CD9AE06', ',ML,MLOps,Microsoft Fabric,Apache Spark,Observability,Monitoring APIs,Spark Applications Comparison,Run Series Analysis,Performance Diagnostics,Spark Advisor,Data Skew Diagnostics,Allocation Reporting,User Data Functions,Python,CI/CD,Roundups,',
    false, false, false, false, false,
    true, false, 32
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
-- weekly-ml-roundup-2025-09-22
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ml-roundup-2025-09-22', 'roundups', 'Weekly Machine Learning Roundup: Learning Paths and Quantum Research',
    'Updates in machine learning center on making advanced AI and quantum computing more accessible, with new resources for beginners and ongoing research projects. Initiatives aim to build practical skills and support foundational research across various ML fields.
<!--excerpt_end-->
## Community-Driven AI Learning for Data Science and ML
Building on last week’s transparency in benchmarking, the focus now is on accessible ML learning for newcomers, featuring Discord sessions using Microsoft’s Data Science and ML for Beginners path. Participants take part in activities using Copilot, Python, Jupyter, and VS Code Data Wrangler, integrating basic knowledge into AI projects.
Live office hours and collaborative peer groups encourage knowledge exchange, matching last week’s benchmarking theme. Prompt cards, notebooks, and hands-on practice now extend to more early-career users, broadening ML engagement.
- [Practical Ways to Use AI in Your Data Science and ML Journey](https://techcommunity.microsoft.com/t5/microsoft-developer-community/practical-ways-to-use-ai-in-your-data-science-and-ml-journey/ba/p/4454764)
## Microsoft Quantum Computing Research Expansion
Following last week’s Azure ND GB200 v6 hardware benchmarking for ML, Microsoft started a quantum research partnership with the University of Maryland, covering hardware/software co-design, benchmarking standards, and error correction. The Microsoft Quantum platform targets reproducible validation and bridging public-private research, reflecting previous ML workflow improvements.
This collaboration paves the way for new programming standards and validation models, continuing the drive for transparent benchmarking from last week.
- [Microsoft Opens Quantum Research Center with Maryland Partnership](https://blogs.microsoft.com/on-the-issues/2025/09/17/our-new-collaboration-with-maryland-will-accelerate-scalable-quantum-computing/)',
    'Updates in machine learning center on making advanced AI and quantum computing more accessible, with new resources for beginners and ongoing research projects. Initiatives aim to build practical skills and support foundational research across various ML fields.',
    1758524400, 'ml', '/ml/roundups/weekly-ml-roundup-2025-09-22', 'TechHub',
    'TechHub', '483F9B037F4C44BC0F83743B1AC84A63CA4A83343F2A12615A8D01B63FAD77B8', ',ML,Data Science,AI Education,Beginner Learning,Community Learning,Discord,GitHub Copilot,Python,Jupyter Notebooks,VS Code,VS Code Data Wrangler,Benchmarking,Quantum Computing,Error Correction,Hardware Software Co Design,Roundups,',
    false, false, false, false, false,
    true, false, 32
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
-- weekly-ml-roundup-2025-09-15
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ml-roundup-2025-09-15', 'roundups', 'Weekly Machine Learning Roundup: MLPerf on Azure GB200 v6',
    'Machine learning developments focus on benchmarking language models using Azure AI hardware, with guides enabling reproducible testing for demanding workloads. Extending last week''s coverage of benchmarking standards, practical instructions offer clear steps for real-world large-model testing.
<!--excerpt_end-->
## Benchmarking Llama 2 70B and Llama 3.1 405B on Azure ND GB200 v6
A comprehensive guide explains benchmarking Llama 2 70B and Llama 3.1 405B models with MLPerf Inference v5.1 on Azure ND GB200 v6 VMs running NVIDIA Grace CPUs and Blackwell B200 GPUs. Detailed steps include VM setup, organizing data, repo cloning, and prepping the environment. Results show Llama 2 70B at 52,000 tokens/sec and Llama 3.1 405B at 847 tokens/sec on a single VM, matching global performance. Sample configurations and MLPerf orchestration enable repeatable evaluations for both research and production.
These outcomes reinforce transparent processes and standards-based evaluation highlighted last week.
- [Benchmarking Llama 2 70B and 405B Models on Azure ND GB200 v6 with MLPerf Inference v5.1](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/a-quick-guide-to-benchmarking-ai-models-on-azure-llama-405b-and/ba-p/4452192)',
    'Machine learning developments focus on benchmarking language models using Azure AI hardware, with guides enabling reproducible testing for demanding workloads. Extending last week''s coverage of benchmarking standards, practical instructions offer clear steps for real-world large-model testing.',
    1757919600, 'ml', '/ml/roundups/weekly-ml-roundup-2025-09-15', 'TechHub',
    'TechHub', '2305CEB84DD9BC4470A495F3937F0A574C47A0208919C867D19F9A9B582FE4B6', ',ML,LLM Benchmarking,MLPerf Inference,MLPerf Inference V5.1,Azure ND GB200 V6,Azure AI Infrastructure,NVIDIA Blackwell B200,NVIDIA Grace CPU,Llama 2 70b,Llama 3.1 405b,Tokens Per Second,Reproducible Testing,Performance Evaluation,High Performance Computing,Inference Optimization,Roundups,',
    false, false, false, false, false,
    true, false, 32
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
-- weekly-ml-roundup-2025-09-01
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ml-roundup-2025-09-01', 'roundups', 'Weekly Machine Learning Roundup: Azure GPU Inference and Fabric Ops',
    'Machine learning updates focus on better LLM inference performance, improvements to cloud productivity, and clear guidance for teams deploying large-scale solutions. Insights include Azure GPU benchmarking for model throughput, real-world diagnostics, and new analytics features in Microsoft Fabric.
<!--excerpt_end-->
## Llama 3.1 8B and DeepSeek R1: Azure GPU Inference Analysis
Following earlier coverage on LLM pretraining optimizations, this week’s benchmarks examine Meta’s Llama 3.1 8B and DeepSeek R1 using Azure ND-H100-v5 GPUs and vLLM. The analysis shows how optimizations like quantization and parallel processing yield throughput improvements of over 38%, and includes comparisons across Azure ND-series hardware for speed, cost, and scalability. DeepSeek R1 is effective for complex tasks, but slower and less cost-efficient than lighter models—helping teams choose the right model for their needs.
- [Benchmarking Llama 3.1 8B AI Inference on Azure ND-H100-v5 with vLLM](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/performance-of-llama-3-1-8b-ai-inference-using-vllm-on-nd-h100/ba-p/4448355)
- [Benchmarking Llama 3.1 8B Inference with vLLM on Azure GPU and CPU VMs](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/inference-performance-of-llama-3-1-8b-using-vllm-across-various/ba-p/4448420)
- [Performance Analysis: DeepSeek R1 Inference with vLLM on Azure ND-H100-v5](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/performance-analysis-of-deepseek-r1-ai-inference-using-vllm-on/ba-p/4449351)
## Productivity and Monitoring Advances in Microsoft Fabric
Microsoft Fabric now offers Fabric Notebooks with direct Pandas DataFrame handling via Apache Arrow, boosting workflow speed and memory efficiency. Monitoring and troubleshooting advances include improved mapping, granular log filtering, and execution snapshots for Spark workloads. The new JobInsight library provides diagnostics and historical analysis, automating insight generation for analytics pipelines.
- [Enhancing Fabric Notebooks: Native Pandas DataFrame Support in User Data Functions](https://blog.fabric.microsoft.com/en-US/blog/now-in-fabric-notebook-udf-integration-with-native-support-for-pandas-dataframes-and-series-via-apache-arrow/)
- [Enhanced Monitoring for Spark High Concurrency Workloads in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/enhanced-monitoring-for-spark-high-concurrency-workloads-in-microsoft-fabric/)
- [Gain Deeper Insights into Spark Jobs with JobInsight in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/gain-deeper-insights-into-spark-jobs-with-jobinsight-in-microsoft-fabric/)
## Practical Fabric Data Engineering: Materialized Lake Views, Community Best Practices
Guides showcase effective Fabric pipeline operations, spotlighting Materialized Lake Views for syncing Azure SQL to OneLake and detailing layered data transformations. Tutorials from Microsoft MVPs and Super Users cover dynamic masking, Power BI, REST admin, Pandas analysis, and efficient transformation patterns, with tips for troubleshooting and certification.
- [Mastering Declarative Data Transformations with Materialized Lake Views](https://blog.fabric.microsoft.com/en-US/blog/mastering-declarative-data-transformations-with-materialized-lake-views/)
- [Fabric Influencers Spotlight: August 2025 Edition](https://blog.fabric.microsoft.com/en-US/blog/fabric-influencers-spotlight-august-2025/)',
    'Machine learning updates focus on better LLM inference performance, improvements to cloud productivity, and clear guidance for teams deploying large-scale solutions. Insights include Azure GPU benchmarking for model throughput, real-world diagnostics, and new analytics features in Microsoft Fabric.',
    1756710000, 'ml', '/ml/roundups/weekly-ml-roundup-2025-09-01', 'TechHub',
    'TechHub', 'E5478CBE514A3D81DEAE1A40A463E54FDD81DDDB4ABC17E3662FE8F0AD408948', ',ML,LLM Inference,Azure GPU,NVIDIA H100,Azure ND H100 V5,Vllm,Quantization,Model Parallelism,Llama 3.1,DeepSeek R1,Benchmarking,Microsoft Fabric,Apache Spark,Pandas,Apache Arrow,Roundups,',
    false, false, false, false, false,
    true, false, 32
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
-- weekly-ml-roundup-2025-08-25
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ml-roundup-2025-08-25', 'roundups', 'Weekly Machine Learning Roundup: Pretraining Scale and Agent Reasoning',
    'This week’s machine learning news focuses on practical improvements to scaling model training and agent development. Developers now have guides for optimizing pretraining at scale, new research on agent reasoning, and updates to data engineering workflows that simplify testing and iteration. The overall direction continues to support efficient ML development and deployment.
<!--excerpt_end-->
## Large-Scale AI Pretraining Optimization on Azure ND GB200 v6
Building on last week’s discussions around cloud-based model training, this week’s benchmarking research provides detailed recommendations for optimizing the pretraining of Llama3 8B models on Azure ND GB200 v6. The study covers adjustments to tensor, pipeline, context, and data parallelism, repeating last week’s strategies for deploying scalable workloads using Azure AKS and vLLM. Benchmarking batch sizes and numerical precision modes, the authors recommend specific parameters for the best throughput: tensor parallelism 1, pipeline parallelism 2, context parallelism 1, and micro batch size 4.
All scripts are shared for reproducibility via the Azure AI Benchmarking Guide, supporting transparent scaling and tuning for teams running production ML on large clusters.
- [Optimizing Large-Scale AI Performance with Pretraining Validation on a Single Azure ND GB200 v6](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/optimizing-large-scale-ai-performance-with-pretraining/ba-p/4445273)
## Feature Updates: Enhanced AI Capability and Developer Workflow
Following on recent analytics and optimizer updates, MindJourney—developed by Microsoft Research—improves spatial reasoning for agents in dynamic, simulated environments. Integrating a pretrained world model and spatial beam search, MindJourney improves agent navigation and accuracy by 8% without requiring agent retraining, with clear uses in robotics, simulation, and accessibility development.
Microsoft Fabric’s new “Develop mode” for User Data Functions now provides a safe editor for testing Python logic before production deployment. This is a direct response to calls for safer, more controlled custom code testing in platforms like Spark, Databricks, and Fabric, and only requires a library update to enable.
- [MindJourney: AI Agents Navigate and Reason in Simulated 3D Worlds](https://www.microsoft.com/en-us/research/blog/mindjourney-enables-ai-to-explore-simulated-3d-worlds-to-improve-spatial-interpretation/)
- [Test and Validate Functions with Develop Mode in Fabric User Data Functions](https://blog.fabric.microsoft.com/en-US/blog/test-and-validate-your-functions-with-develop-mode-in-fabric-user-data-functions-preview/)',
    'This week’s machine learning news focuses on practical improvements to scaling model training and agent development. Developers now have guides for optimizing pretraining at scale, new research on agent reasoning, and updates to data engineering workflows that simplify testing and iteration. The overall direction continues to support efficient ML development and deployment.',
    1756105200, 'ml', '/ml/roundups/weekly-ml-roundup-2025-08-25', 'TechHub',
    'TechHub', '6BA517D33169830182DEC0397D2B54A9A26ADB27C9C48AE442222270DBE61E2B', ',ML,Large Scale Training,Pretraining,Benchmarking,Azure ND GB200 V6,Azure AI,Llama 3,Distributed Training,Tensor Parallelism,Pipeline Parallelism,Vllm,AKS,AI Agents,Spatial Reasoning,Microsoft Fabric,Roundups,',
    false, false, false, false, false,
    true, false, 32
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
-- weekly-ml-roundup-2025-08-18
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-ml-roundup-2025-08-18', 'roundups', 'Weekly Machine Learning Roundup: Open Models, Lakehouses, Faster Training',
    'Machine learning updates this week include expanded cloud support for open models, step-by-step LLM deployment, scalable optimizers, and upgraded analytics tools. Microsoft introduced more open-source, cloud-native, and production-friendly options, while new tools like Dion are making large model training more efficient. Companies are highlighting useful deployment strategies and tuning guidance so teams can deliver quality ML systems with less friction.
<!--excerpt_end-->
## Cloud-Native LLM Deployment and Optimization
A new, comprehensive guide walks through deploying OpenAI’s GPT-OSS-20B model on Azure Kubernetes Service (AKS) with KAITO and vLLM, using managed GPUs for scalable and reproducible inference. The tutorial covers everything from setting up clusters to benchmarking, making it easier for teams to roll out open LLMs in Azure environments.
- [Deploying OpenAI’s GPT-OSS-20B on Azure AKS with KAITO and vLLM](https://techcommunity.microsoft.com/t5/ai-machine-learning-blog/deploying-openai-s-first-open-source-model-on-azure-aks-with/ba-p/4444234)
## Innovations in Data Lake Interoperability
Microsoft Fabric''s OneLake now lets you access Delta Lake tables as Apache Iceberg format using Apache XTable. This enables analytics engines such as Spark, Trino, or Snowflake to work with lake data without ETL or duplication, advancing Microsoft’s vision for a more flexible, open lakehouse platform.
- [How Microsoft OneLake Seamlessly Provides Apache Iceberg Support for All Fabric Data](https://blog.fabric.microsoft.com/en-US/blog/how-to-access-your-microsoft-fabric-tables-in-apache-iceberg-format/)
## Advances in Distributed Optimization for AI Model Training
Microsoft Research introduced Dion, a distributed optimizer for training massive models like LLaMA-3 405B. Dion leverages orthonormal updates to make optimizer steps up to 10x faster while preserving accuracy, and works well with distributed training frameworks such as FSDP2 and tensor parallelism.
- [Microsoft Releases Dion: A New Scalable Optimizer for Training AI Models](https://www.microsoft.com/en-us/research/blog/dion-the-distributed-orthonormal-update-revolution-is-here/)
## Practical Data Engineering and Analytics Platform Enhancements
A deep dive into the Spark UI offers practical advice for improving job run times, fixing data skew and joins, and spotting garbage collection issues—especially for Databricks users seeking to move past trial-and-error tuning.
- [A Deep Dive into Spark UI for Job Optimization](https://techcommunity.microsoft.com/t5/microsoft-mission-critical-blog/a-deep-dive-into-spark-ui-for-job-optimization/ba-p/4442229)
Microsoft Fabric''s Copy Job feature now supports table-level incremental resets, automatic destination table creation, and JSON files—streamlining ETL pipeline deployment by reducing manual steps.
- [Enhancements to Microsoft Fabric Copy Job: Reset Incremental Copy, Auto Table Creation, and JSON Support](https://blog.fabric.microsoft.com/en-US/blog/simplifying-data-ingestion-with-copy-job-reset-incremental-copy-auto-table-creation-and-json-format-support/)
Azure Essentials Show featured Databricks, highlighting unified analytics, ML lifecycle support, and integration across the Azure platform—useful for developers building new skills for Azure ML environments.
- [Supercharge Data and AI Innovation with Azure Databricks](/ai/videos/supercharge-data-and-ai-innovation-with-azure-databricks)
## Enterprise ML Transformation and Modern DataOps
A case study from Adastra and Heritage Grocers Group illustrates how Microsoft Fabric and Azure OpenAI unified post-acquisition data, powered predictive analytics, and rolled out a working system in just six months, showing real benefits from a modern, cloud-based ML setup.
- [How Adastra Used Microsoft Fabric and Azure OpenAI Service to Transform Heritage Grocers Group’s Data Analytics](https://techcommunity.microsoft.com/t5/partner-news/partner-case-study-adastra/ba-p/4442288)
## Other ML News
Excel’s 40th anniversary content showcases its transformation into a capable platform for analytics and ML, including expanded modeling support, Power BI linkage, and deeper connection to Microsoft Fabric.
- [Excel at 40 Week 1: Days 1–3](https://techcommunity.microsoft.com/t5/excel/excel-at-40-week-1-days-1-3/m-p/4443674#M254078)',
    'Machine learning updates this week include expanded cloud support for open models, step-by-step LLM deployment, scalable optimizers, and upgraded analytics tools. Microsoft introduced more open-source, cloud-native, and production-friendly options, while new tools like Dion are making large model training more efficient. Companies are highlighting useful deployment strategies and tuning guidance so teams can deliver quality ML systems with less friction.',
    1755500400, 'ml', '/ml/roundups/weekly-ml-roundup-2025-08-18', 'TechHub',
    'TechHub', '84C17E082B6244361613D4B7C21E059B27F9F014C3C358F3CDC043D09F5A008C', ',ML,LLMs,OpenAI GPT OSS 20b,AKS,KAITO,Vllm,Managed GPUs,Microsoft Fabric,OneLake,Delta Lake,Apache Iceberg,Apache XTable,Distributed Training,Dion Optimizer,Apache Spark,Roundups,',
    false, false, false, false, false,
    true, false, 32
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
