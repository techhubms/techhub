[![nighthawk cover small image](https://devblogs.microsoft.com/all-things-azure/wp-content/uploads/sites/83/2026/04/nighthawk-cover-small-300x289.webp)](https://devblogs.microsoft.com/all-things-azure/wp-content/uploads/sites/83/2026/04/nighthawk-cover-small.webp)

If you work in field engineering, you know the scenario. A customer is deploying AKS in a regulated environment. They hit an issue during node bootstrapping. They want to know exactly what happens when a node joins the cluster, which components run in which order, and whether the behaviour they’re seeing is expected. The question sounds simple. The answer is not.

The answer is spread across half a dozen places at once. It’s in the source code: AgentBaker, the node controller, cloud-provider-azure. It’s in a Microsoft Learn article that’s technically correct but three levels of abstraction above what actually runs on the node. It’s in the release notes buried in a changelog. It’s in the institutional knowledge of a teammate who worked on that feature and may or may not be on Teams right now. Assembling a reliable answer means pulling all of that together, reconciling it, and communicating it clearly, ideally in writing that someone else can use later.

That’s the job. And it doesn’t scale.

The knowledge required to do field engineering well grows faster than any individual can absorb it. Services change. Networking models evolve. Identity patterns that were best practice eighteen months ago have been superseded. The expertise exists in the ecosystem, in repos, in docs, in release notes, in the people who built the thing, but the cost of retrieving it, correlating it, and turning it into something actionable is high. And when you do find the answer, it usually lives in your head or a Teams thread that ages out in a week.

Project Nighthawk is built to close that gap. Ray Kao and I built it specifically for our work as Global Black Belts: the AKS and ARO questions we field every week require a level of depth and precision that general-purpose AI assistants consistently fall short on.

## What Nighthawk Is

Nighthawk is a multi-agent research system built directly inside VS Code with GitHub Copilot. The core idea is simple: field expertise is not just about knowing things, it’s about being able to retrieve, verify, and communicate the right things quickly. Nighthawk handles the retrieval and verification so you can focus on the communication and the judgment.

You ask it a technical question about AKS or Azure Red Hat OpenShift, and it produces a fact-checked, source-cited technical report in markdown. Not a summary of what a language model remembers from training data. An actual investigation: source code read, official documentation consulted, claims verified, findings written up. The kind of report a senior engineer would produce after two hours of focused research, delivered in a fraction of the time.

The entry point is intentionally simple:

```
/Nighthawk How does AKS implement KMS encryption with customer-managed keys?

```

Behind that single command is a six-agent pipeline that classifies the question, researches it against live source code and official documentation, synthesizes findings into a structured report, and validates every claim before it lands in the `notes/` directory.

## The Problem with Asking AI to Research

Asking a language model directly runs into a predictable problem. LLMs are excellent at synthesizing patterns from training data, but Azure is a moving target. Source code changes. Features ship. Behaviors differ between versions. A model trained six months ago may confidently describe a code path that was refactored in the last release. For general background knowledge, this is usually fine. For the kind of precise, version-specific, behaviorally accurate answers that field engineering demands, it is not.

The solution is grounding. Nighthawk doesn’t ask the model to recall what it knows about AKS kubelet bootstrapping. It directs researcher agents to search the locally cloned AgentBaker repository, read the relevant code, cross-reference the Microsoft Learn documentation, and report what they actually find. Source code is one input. Official docs are another. Release notes and changelogs are another. The researcher correlates all of them and surfaces conflicts when they exist. That’s closer to how a good engineer actually investigates a problem, and it produces answers that hold up when a customer asks a follow-up.

This required a specific architectural choice: Nighthawk researchers operate against locally cloned repositories. Before a research session, you run a one-time setup:

```bash
git clone --depth=1 https://github.com/Azure/AgentBaker.git repos/AgentBaker
git clone --depth=1 https://github.com/Azure/AKS.git repos/AKS
git clone --depth=1 https://github.com/kubernetes-sigs/cloud-provider-azure.git repos/cloud-provider-azure
```

Before each research run, the researcher agent pulls the latest:

```bash
git -C repos/AgentBaker pull --ff-only
```

The model is now working against the actual current state of the codebase, not a memory of it.

## Six Agents, One Pipeline

Nighthawk implements the **Agent Handoff Pattern** as described in the [Azure Architecture Center AI Agent Design Patterns guide](https://learn.microsoft.com/en-us/azure/architecture/ai-ml/guide/ai-agent-design-patterns#agent-handoff-pattern-example): specialized agents complete distinct tasks and pass results to the next agent through well-defined contracts. No single agent tries to do everything; each one is scoped, and the quality of the final output depends on that separation.

```
/Nighthawk question
 |
 v
 [Orchestrator] -.md

```

**Orchestrator** coordinates the run. It reads the question, invokes the Classifier, routes to the right Researcher, hands off to the Synthesizer, and triggers the FactChecker. It’s the glue.

**Classifier** determines which service the question is about (AKS or ARO), what type of question it is (architecture, bug, guidance), and extracts keywords that researchers will use to focus their search. This matters because AKS and ARO have different source repos, different team structures, and very different implementation patterns.

**Researchers** (separate agents for AKS and ARO) do the heavy lifting. They read the Nighthawk-LocalRepos skill to understand exactly which repositories to search and in which order. They use `grep_search` to find relevant code, `read_file` to examine it in depth, and the Microsoft Learn MCP server to pull in official documentation. MCP (Model Context Protocol) is what gives the researcher agent structured, tool-mediated access to external knowledge sources without leaving the VS Code context. The output is structured research notes, not a report. That comes later.

**Synthesizer** takes the research notes and writes the actual report. It reads the Nighthawk-ReportTemplates skill before writing, which defines three report formats (architecture, bug, guidance) with specific sections: TL;DR, Technical Deep Dive, Key Findings, and References. Where a concept benefits from a visual — a flow, a component relationship, a decision tree — the Synthesizer generates a Mermaid diagram inline. The structure is consistent because the template is encoded, not left to model discretion.

**FactChecker** is the quality gate. It reads the finished report and validates each factual claim against the cited sources. Claims that can be verified get a checkmark. Claims that can’t get flagged. The summary includes a count of verified and unverified claims so the person sharing the report with a customer knows exactly where to look before they do.

## What a Report Looks Like

The output for a question like “What are the required permissions for Terraform-based AKS deployment?” looks like this:

- **TL;DR**: One or two sentences with the direct answer, no hedging

- **Recommendations table**: Specific roles mapped to scope and reason

- **Terraform examples**: Real HCL with working configuration patterns

- **Feature-specific guidance**: What changes when you add BYO VNet, private DNS, ACR, or workload identity

- **Mermaid diagrams**: Architecture flows, component relationships, and decision trees rendered inline where they add clarity

- **Reference table**: Complete list of Microsoft Learn links and GitHub file paths used

- **Fact-check summary**: Claim counts and any flagged items for review

You can see a full example in the [notes/ directory](../notes/Nighthawk-2026-03-31-AKS-Terraform-Permissions.md). That report was generated by Nighthawk in a single run from the `/Nighthawk` command.

## Skills: Encoding Expertise as Reusable Instructions

One of the more interesting design choices in Nighthawk is the use of VS Code agent skills to codify workflow knowledge. Skills are markdown files that agents read at the start of a run to understand their operating context.

The `Nighthawk-LocalRepos` skill tells every researcher agent exactly which repositories exist locally, what each one is for, and why running `git pull` before research is mandatory. Agents don’t need this information in their system prompt; they load it on demand, which keeps the core agent definitions focused.

The `Nighthawk-ReportTemplates` skill gives the Synthesizer the exact structure for each report type, writing guidelines, and Mermaid diagram conventions. The result is consistent report structure across every research run regardless of which question was asked.

This pattern generalizes well beyond Nighthawk. Skills are a clean way to separate durable domain knowledge from the agent definition itself. The agent knows how to reason; the skill tells it what to reason about in this specific context.

## What Nighthawk Is Not

Nighthawk is not a general-purpose AI assistant for Azure. It’s a research pipeline designed for a specific use case: a field engineer needs a deep technical report on a narrow AKS or ARO topic, and they need it grounded in verifiable sources. It doesn’t replace the judgment that comes from years of working with the platform; it gives that judgment better raw material to work with.

It doesn’t browse the web. It doesn’t query live Azure APIs. And it’s deliberately scoped to AKS and ARO because building a quality system for a focused domain is more useful than building a mediocre one for everything. Field expertise is domain-specific, and so is Nighthawk.

Adding support for a new Azure service means creating a new researcher agent following the existing pattern. The architecture is designed for that kind of extension; each researcher is isolated and follows the same research contract.

## Getting Started

If you have VS Code with GitHub Copilot and access to the repository, you’re ready. Clone the repos once, enable the required tools in the VS Code chat panel, and run:

```
/Nighthawk What are the networking options for ARO private clusters?

```

The full setup guide is in [USAGE.md](../USAGE.md).

## A Note on the Architecture Decision

We wrote up the rationale for first-principles design choices in [ARCHITECTURE-DECISION-FRAMEWORK.md](../ARCHITECTURE-DECISION-FRAMEWORK.md). If you’re building your own multi-agent system and want to understand why we made specific tradeoffs (why local repos over MCP, why six specialized agents over one general one, why separate Synthesizer and FactChecker stages), that’s where to look.

The short version: quality comes from constraints. A researcher that can only search a defined set of repos produces more accurate output than one that can search anywhere. A FactChecker that runs after synthesis catches more errors than one baked into the synthesis stage. Separation of concerns applies to agents as much as it does to software.

The repository is at [https://github.com/microsoftgbb/project-nighthawk](https://github.com/your-org/project-nighthawk). Clone it, run a question, and see what comes back. If you work with AKS or ARO customers regularly, the time savings become obvious fast.

 
 

Category

## Author

![Diego Casati](https://devblogs.microsoft.com/all-things-azure/wp-content/uploads/sites/83/2024/10/Casati-96x96.webp)

Migrate and Modernize Global Black Belt

Azure Global Black Belt team with 18+ years in cloud and software engineering, specializing in Kubernetes, platform engineering, and cloud-native architectures for high-performance workloads. Outside of work, he enjoys camping, electronics tinkering, and off-grid radio in the Canadian Rockies.

![Ray Kao](https://devblogs.microsoft.com/all-things-azure/wp-content/uploads/sites/83/2025/02/profile-96x96.webp)

Award eligible primate. AI DevTools & Platform Engineering on the Microsoft Global Black Belt Team