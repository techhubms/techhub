---
feed_name: Microsoft Security Blog
author: Angela Argentati, Matthew Dressman, Habiba Mohamed and Microsoft AI Security
section_names:
- ai
- azure
- security
primary_section: ai
title: 'Observability for AI Systems: Strengthening visibility for proactive risk detection'
date: 2026-03-18 16:00:00 +00:00
tags:
- Agentic AI
- AI
- AI Observability
- Application Insights
- Azure
- Azure Monitor
- Behavioral Baselines
- Conversation Correlation
- Crescendo
- Data Exfiltration
- Evaluation
- Forensic Reconstruction
- GenAI Semantic Conventions
- Generative AI
- Governance
- Guardrails
- Indirect Prompt Injection
- Logs Metrics Traces
- Microsoft Agent 365
- Microsoft Agent 365 Observability SDK
- Microsoft Foundry Agent Tracing
- Microsoft Foundry Control Plane
- Multi Turn Jailbreaks
- News
- OpenTelemetry
- Policy Enforcement
- Prompt Injection
- Secure Development Lifecycle (sdl)
- Security
- Telemetry
- Tracing
external_url: https://www.microsoft.com/en-us/security/blog/2026/03/18/observability-ai-systems-strengthening-visibility-proactive-risk-detection/
---

Angela Argentati, Matthew Dressman, Habiba Mohamed, and Microsoft AI Security explain why AI observability (beyond classic uptime/latency monitoring) is now a core requirement for securing GenAI and agentic systems, and outline concrete SDL steps—instrumentation, context capture, baselines, and governance—using tools like OpenTelemetry and Azure Monitor.<!--excerpt_end-->

# Observability for AI Systems: Strengthening visibility for proactive risk detection

Adoption of generative AI (GenAI) and agentic AI has moved from experimentation to real enterprise deployments. These systems can interact with sensitive data, call external APIs, trigger workflows, and collaborate with other agents—so teams need continuous visibility into how they behave in production to detect risk, validate policy adherence, and maintain operational control.

Microsoft positions **observability** as a foundational security and governance requirement for AI systems in production.

- Background reference: [Observability (Microsoft guidance)](https://learn.microsoft.com/en-us/security/zero-trust/sfi/observability-ai-systems)
- Related Microsoft Security Blog posts:
  - [Microsoft SDL evolving security practices for an AI-powered world](https://www.microsoft.com/en-us/security/blog/2026/02/03/microsoft-sdl-evolving-security-practices-for-an-ai-powered-world/)
  - [Secure Agentic AI for Your Frontier Transformation](https://www.microsoft.com/en-us/security/blog/2026/03/09/secure-agentic-ai-for-your-frontier-transformation/)

## Observability for AI systems

Traditional software observability works well when systems are deterministic: client apps call structured APIs and services execute predictable code paths. In that world, standard metrics like latency, error rates, and throughput often provide enough signal.

GenAI and agentic systems are **probabilistic** and can make complex decisions as they run. That changes what teams need to collect and how they reason about failures and compromise.

### Example failure mode: trust-boundary compromise without “red” health metrics

A scenario described in the post:

1. An email agent asks a research agent to fetch something on the web.
2. The research agent retrieves a page containing hidden malicious instructions.
3. The poisoned content is passed back as trusted input.
4. The email agent then forwards sensitive documents to unauthorized recipients (data exfiltration).

Classic monitoring can remain “green” (no failures, no errors), even though a security boundary between untrusted content and trusted context was compromised.

To detect and investigate this, teams need visibility into **how context was assembled** at each step (what was retrieved, how it influenced model behavior, and how it propagated across agents).

## Traditional observability versus AI observability

The post defines AI observability as end-to-end ability to monitor, understand, and troubleshoot AI systems across:

- development
- evaluation
- deployment
- operation

A key shift: in AI systems, the “input” isn’t just a bounded, schema-defined request. It’s assembled **context**, including:

- natural language instructions
- system and developer instructions
- conversation history
- tool outputs
- retrieved content (web pages, emails, documents, tickets)

### Context capture is central

Best practice: capture which input components were assembled for each run, including:

- source provenance
- trust classification
- the resulting system outputs

### Correlation needs to span conversations, not single requests

Traditional tracing often assumes one request maps to one outcome. In AI systems, failures can unfold across turns; each step may look benign until escalation results in disallowed output.

The post references multi-turn jailbreaks such as [Crescendo](https://arxiv.org/abs/2404.01833).

Best practice: propagate a stable **conversation identifier** across turns and preserve trace context end-to-end—what the authors call **agent lifecycle-level correlation** (matching the span of persistent memory/state).

## Defining AI system observability

Traditional observability is based on **logs, metrics, and traces**. Those still apply, but what needs to be captured changes to include AI-native signals.

### Logs

Logs should capture data about the interaction, such as:

- request identity context
- timestamp
- user prompts and model responses
- which agents/tools were invoked
- which data sources were consulted

The post emphasizes that prompt/response logs can be early signals of novel attacks (before signatures exist) and are essential for:

- identifying multi-turn escalation
- verifying whether attacks changed system behavior
- adjudicating safety detections
- reconstructing attack paths

### Metrics

Metrics should include both classic and AI-specific signals:

- latency/response times/errors
- token usage
- agent turns
- retrieval volume

These can help identify unauthorized usage or behavior changes due to model updates.

### Traces

Traces capture the ordered sequence of execution events from initial prompt through response generation. Without traces, debugging agent failures becomes guesswork.

### Two additional components: evaluation and governance

The post adds two core components beyond logs/metrics/traces:

- **Evaluation**: measure response quality, grounding in source material, and correct tool usage; helps quantify reliability, instruction alignment, and operational risk.
- **Governance**: measure/verify/enforce acceptable behavior using observable evidence; supports policy enforcement, auditability, and accountability.

## Operationalizing AI observability through the SDL

The authors propose five steps to make observability a secure-development control (not an optional practice).

1. **Incorporate AI observability into secure development standards**
   - Treat observability as a lifecycle requirement for GenAI/agentic systems.
2. **Instrument from the start of development**
   - Use industry conventions such as OpenTelemetry (OTel) and its [GenAI semantic conventions](https://opentelemetry.io/docs/specs/semconv/gen-ai/).
   - Platform-native options mentioned:
     - [Microsoft Foundry agent tracing (preview)](https://learn.microsoft.com/en-us/azure/foundry/observability/concepts/trace-agent-concept)
     - [Microsoft Agent 365 Observability SDK (Frontier preview)](https://learn.microsoft.com/en-us/microsoft-agent-365/admin/monitor-agents)
3. **Capture the full context**
   - Log prompts/responses, retrieval provenance, tools invoked, arguments passed, and permissions in effect.
   - Govern capture/retention with data contracts balancing forensic needs vs privacy, residency, retention, and regulatory obligations; align access controls and encryption with enterprise policy.
4. **Establish behavioral baselines and alert on deviation**
   - Capture normal activity patterns (tool call frequency, retrieval volumes, token consumption, evaluation score distributions).
   - Use services such as:
     - [Azure Monitor](https://learn.microsoft.com/en-us/azure/azure-monitor/fundamentals/overview)
     - [Application Insights](https://learn.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview)
   - Alert on meaningful deviations vs static thresholds.
5. **Manage enterprise AI agents**
   - Observability alone isn’t enough; teams need inventory and consistent enforcement.
   - Governance examples:
     - [Microsoft Foundry Control Plane](https://learn.microsoft.com/en-us/azure/foundry/control-plane/overview?view=foundry) (inventory, observability, compliance/guardrails, security)
     - [Microsoft Agent 365](https://www.microsoft.com/en-us/microsoft-agent-365) (tenant-level governance in the Microsoft 365 admin plane; Frontier preview)

## Benefits for security teams

The post argues that making AI systems observable turns opaque model behavior into actionable security signals, improving both:

- proactive detection
- reactive incident investigation

When embedded in the SDL, observability becomes an engineering control:

- define data contracts early
- instrument during design/build
- verify before release that detection and incident response needs are met

Security testing can validate that scenarios like indirect prompt injection or tool-mediated data exfiltration are surfaced by runtime protections, and that logs/traces enable end-to-end reconstruction of event paths, impact, and control decisions.

The authors note that many organizations already deploy inference-time protections (for example, [Microsoft Foundry guardrails and controls](https://learn.microsoft.com/en-us/azure/foundry/guardrails/guardrails-overview)). Observability complements these with faster incident reconstruction, clearer impact analysis, and measurable improvement over time.

Finally, they argue teams usually don’t need to reinvent monitoring—often they can extend known instrumentation practices to include AI-specific signals, establish behavioral baselines, and test detectability.

## Related links

- Original post: [Observability for AI Systems: Strengthening visibility for proactive risk detection](https://www.microsoft.com/en-us/security/blog/2026/03/18/observability-ai-systems-strengthening-visibility-proactive-risk-detection/)
- Microsoft guidance: [Observability](https://learn.microsoft.com/en-us/security/zero-trust/sfi/observability-ai-systems)


[Read the entire article](https://www.microsoft.com/en-us/security/blog/2026/03/18/observability-ai-systems-strengthening-visibility-proactive-risk-detection/)

