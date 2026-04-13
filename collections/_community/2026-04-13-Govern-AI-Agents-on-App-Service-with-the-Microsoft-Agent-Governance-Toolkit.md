---
primary_section: ai
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/govern-ai-agents-on-app-service-with-the-microsoft-agent/ba-p/4510962
title: Govern AI Agents on App Service with the Microsoft Agent Governance Toolkit
feed_name: Microsoft Tech Community
author: jordanselig
date: 2026-04-13 21:15:44 +00:00
section_names:
- ai
- azure
- dotnet
- security
tags:
- .NET
- Agent Governance
- AI
- Application Insights
- ASP.NET Core
- Azure
- Azure App Service
- Azure Monitor
- Circuit Breakers
- Community
- Compliance as Code
- DefaultAzureCredential
- Deployment Slots
- Easy Auth
- Error Budgets
- EU AI Act
- KQL
- Managed Identity
- Microsoft Agent Framework
- Microsoft Agent Governance Toolkit
- Multi Agent Systems
- NuGet
- OpenTelemetry
- OWASP Top 10 For Agentic Applications
- Policy Enforcement
- Private Endpoints
- Prompt Injection
- Security
- SLOs
- Tool Allowlists
- VNet Integration
- YAML Policies
---

jordanselig shows how to add runtime governance to a multi-agent ASP.NET Core travel planner on Azure App Service using the Microsoft Agent Governance Toolkit, including YAML policy allowlists, audit logging into Application Insights, and SRE controls like SLOs and circuit breakers.<!--excerpt_end-->

# Govern AI Agents on App Service with the Microsoft Agent Governance Toolkit

## Context: Part 3 of 3 — Multi-Agent AI on Azure App Service

This post continues a series:

1. [Blog 1: Build](https://techcommunity.microsoft.com/blog/appsonazureblog/build-multi-agent-ai-apps-on-azure-app-service-with-microsoft-agent-framework-1-/4510017) — Build a multi-agent travel planner with Microsoft Agent Framework (MAF) 1.0 on Azure App Service
2. [Blog 2: Monitor](https://techcommunity.microsoft.com/blog/appsonazureblog/monitor-ai-agents-on-app-service-with-opentelemetry-and-the-new-application-insi/4510023) — Add OpenTelemetry and the Application Insights “Agents view”
3. Blog 3: Govern — Add runtime governance with the Microsoft Agent Governance Toolkit (this post)

This post assumes you’ve already deployed the travel planner from Blog 1.

## The governance gap (why this matters in production)

The travel planner is observable, but the key customer question is:

> How do I make sure my agents don't do something they shouldn't?

The example app uses six agents (Coordinator, Currency Converter, Weather Advisor, Local Knowledge, Itinerary Planner, Budget Optimizer). Because they can call tools/APIs and process user data, the risk surface includes:

- Unauthorized API calls (data leakage or unexpected cost)
- Sensitive data exposure (e.g., PII sent to third parties)
- Runaway token spend (recursive loops)
- Tool misuse (prompt injection leading to unintended tool calls)
- Cascading failures across agents

The post points to OWASP’s Top 10 for Agentic Applications (Dec 2025):

- https://genai.owasp.org/resource/owasp-top-10-for-agentic-applications-for-2026/

It also mentions upcoming regulatory enforcement dates:

- EU AI Act high-risk obligations: August 2026
- Colorado AI Act: June 2026

## What the Agent Governance Toolkit does

The [Microsoft Agent Governance Toolkit](https://github.com/microsoft/agent-governance-toolkit) is an MIT-licensed open-source project for runtime governance of autonomous agents, focused on deterministic policy enforcement and auditability.

It’s described as addressing the “OWASP Top 10 for agentic AI risks” with sub-millisecond policy checks.

### Toolkit components (7 packages)

| Package | What it does | Think of it as... |
| --- | --- | --- |
| Agent OS | Stateless policy engine; intercepts actions before execution (reported <0.1ms p99) | Kernel for AI agents |
| Agent Mesh | Cryptographic identity (DIDs), inter-agent trust protocol, dynamic trust scoring | mTLS for agents |
| Agent Runtime | Execution rings, saga orchestration, kill switch | Process isolation |
| Agent SRE | SLOs, error budgets, circuit breakers, chaos engineering | SRE practices |
| Agent Compliance | Governance verification, regulatory mapping (EU AI Act, HIPAA, SOC2) | Compliance-as-code |
| Agent Marketplace | Plugin lifecycle, Ed25519 signing, supply-chain security | Package manager security |
| Agent Lightning | RL training governance with policy-enforced runners | Safe training guardrails |

The toolkit is available in Python, TypeScript, Rust, Go, and .NET. It’s positioned as framework-agnostic (works with Microsoft Agent Framework, LangChain, CrewAI, Google ADK, etc.).

This blog focuses on:

- Agent OS (policy enforcement)
- Agent Compliance (reg mapping + audit trail)
- Agent SRE (SLOs, circuit breakers)

## Adding governance to the ASP.NET Core travel planner (under ~30 minutes)

### Step 1: Add the NuGet package

In `TravelPlanner.Shared.csproj`, add the governance package:

```xml
<ItemGroup>
  <!-- Existing packages -->
  <PackageReference Include="Azure.Monitor.OpenTelemetry.AspNetCore" Version="1.3.0" />
  <PackageReference Include="Microsoft.Agents.AI" Version="1.0.0" />

  <!-- NEW: Agent Governance Toolkit (single package, all features included) -->
  <PackageReference Include="Microsoft.AgentGovernance" Version="3.0.2" />
</ItemGroup>
```

### Step 2: Create a YAML policy file

Create `governance-policies.yaml` in the project root.

This example uses `defaultAction: deny` and then allowlists specific tools:

```yaml
apiVersion: governance.toolkit/v1
name: travel-planner-governance
description: Policy enforcement for the multi-agent travel planner on App Service
scope: global
defaultAction: deny
rules:
  - name: allow-currency-conversion
    condition: "tool == 'ConvertCurrency'"
    action: allow
    priority: 10
    description: Allow Currency Converter agent to call Frankfurter exchange rate API

  - name: allow-weather-forecast
    condition: "tool == 'GetWeatherForecast'"
    action: allow
    priority: 10
    description: Allow Weather Advisor agent to call NWS forecast API

  - name: allow-weather-alerts
    condition: "tool == 'GetWeatherAlerts'"
    action: allow
    priority: 10
    description: Allow Weather Advisor agent to check NWS weather alerts
```

### Step 3: Add governance to the agent pipeline (BaseAgent.cs)

Before:

```csharp
Agent = new ChatClientAgent(
    chatClient,
    instructions: Instructions,
    name: AgentName,
    description: Description)
  .AsBuilder()
  .UseOpenTelemetry(sourceName: AgentName)
  .Build();
```

After (use DI to resolve the governance kernel optionally):

```csharp
var kernel = serviceProvider.GetService<GovernanceKernel>();
if (kernel is not null)
    builder.UseGovernance(kernel, AgentName);

Agent = builder.Build();
```

The `.UseGovernance(kernel, AgentName)` hook intercepts tool/function invocations and evaluates them against the loaded policy before execution.

### Step 4: Register the GovernanceKernel in Program.cs

Load the YAML policy and register the kernel as a singleton if the file exists.

```csharp
using AgentGovernance;

// ... existing builder setup ...

// Configure OpenTelemetry with Azure Monitor (existing — from Blog 2)
builder.Services.AddOpenTelemetry().UseAzureMonitor();

// NEW: Configure Agent Governance Toolkit
var policyPath = Path.Combine(builder.Environment.ContentRootPath, "governance-policies.yaml");
if (File.Exists(policyPath))
{
    try
    {
        var yaml = File.ReadAllText(policyPath);
        var kernel = new GovernanceKernel(new GovernanceOptions
        {
            EnableAudit = true,
            EnableMetrics = true
        });

        kernel.LoadPolicyFromYaml(yaml);
        builder.Services.AddSingleton(kernel);
        Console.WriteLine($"[Governance] Loaded policies from {policyPath}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[Governance] Failed to load: {ex.Message}. Running without governance.");
    }
}
```

## Policy language: key capabilities

### API allowlists / blocklists

With `defaultAction: deny`, any tool call not explicitly allowed gets blocked.

Example blocked log line:

```text
[Governance] Tool call 'DeleteDatabase' blocked for agent 'LocalKnowledgeAgent': No matching rules; default action is deny.
```

### Condition language

Examples shown include matching on tool, agent, OR logic:

```yaml
condition: "tool == 'ConvertCurrency'"
condition: "tool == 'GetWeatherForecast' or tool == 'GetWeatherAlerts'"
condition: "agent == 'CurrencyConverterAgent' and tool == 'ConvertCurrency'"
```

### Priority and conflict resolution

Higher priority wins. This lets you layer broad allows with targeted denies:

```yaml
rules:
  - name: allow-all-weather-tools
    condition: "tool == 'GetWeatherForecast' or tool == 'GetWeatherAlerts'"
    action: allow
    priority: 10

  - name: block-during-maintenance
    condition: "tool == 'GetWeatherForecast'"
    action: deny
    priority: 100
    description: Temporarily block NWS calls during API maintenance
```

### Advanced: OPA Rego and Cedar

The toolkit also supports OPA Rego and Cedar policies. Example Rego snippet (time-based access control):

```rego
package travel_planner.governance

default allow_tool_call = false

allow_tool_call {
  input.agent == "CurrencyConverterAgent"
  input.tool == "get_exchange_rate"
  time.weekday(time.now_ns()) != "Sunday" # Markets closed
}
```

## Why Azure App Service is positioned as a good host for governed agents

The post’s argument: governance is application-level, but production workloads also need platform controls. App Service features called out:

- Managed Identity (avoid secrets; travel planner uses `DefaultAzureCredential` for Azure OpenAI, Cosmos DB, Service Bus)
- VNet Integration + Private Endpoints (network boundaries / defense in depth)
- Easy Auth (built-in authentication in front of your API)
- Deployment Slots (test new `governance-policies.yaml` in staging before swapping)
- Application Insights integration (governance audit events appear alongside OTel traces)
- Always-on + WebJobs (keep long-running workflows warm)
- Azure Developer CLI deployment:

```bash
azd up
```

## Governance audit events in Application Insights

Governance decisions are emitted as trace/span attributes so they show inline in the Agents view.

Examples of audit events:

- Policy: api-allowlist → ALLOWED
- Policy: token-budget → ALLOWED
- Policy: rate-limit → THROTTLED

### KQL: find policy violations in the last 24 hours

```kusto
traces
| where timestamp > ago(24h)
| where customDimensions["governance.decision"] != "ALLOWED"
| extend agentName = tostring(customDimensions["agent.name"]),
         policyName = tostring(customDimensions["governance.policy"]),
         decision = tostring(customDimensions["governance.decision"]),
         violationReason = tostring(customDimensions["governance.reason"]),
         targetUrl = tostring(customDimensions["tool.target_url"])
| project timestamp, agentName, policyName, decision, violationReason, targetUrl
| order by timestamp desc
```

### KQL: track token budget consumption across agents (last hour)

```kusto
customMetrics
| where timestamp > ago(1h)
| where name == "governance.tokens.consumed"
| extend agentName = tostring(customDimensions["agent.name"])
| summarize totalTokens = sum(value),
            avgTokensPerRequest = avg(value),
            maxTokensPerRequest = max(value)
  by agentName, bin(timestamp, 5m)
| order by totalTokens desc
```

## SRE for agents (Agent SRE package)

### Service Level Objectives (SLOs)

Example SLO definition:

```yaml
slos:
  - name: weather-agent-latency
    agent: "WeatherAdvisorAgent"
    metric: latency-p99
    target: 5000ms
    window: 5m
```

### Circuit breakers

Example circuit breaker policy:

```yaml
circuit-breakers:
  - agent: "*"
    failure-threshold: 5
    recovery-timeout: 30s
    half-open-max-calls: 2
```

### Error budgets

The post describes using error budgets to reduce autonomy when reliability drops (e.g., require human approval for high-risk actions until recovery).

## Architecture overview

The described architecture places a transparent governance layer in the agent pipeline:

- Azure App Service hosts the frontend + ASP.NET Core API
- Each agent emits OpenTelemetry traces
- Governance engine intercepts tool calls based on policies
- App Insights receives traces and governance audit events
- Only explicitly allowed external APIs are reachable (with additional network controls via VNet/Private Endpoints)

## Bringing governance to other agent frameworks

Examples are provided for other ecosystems:

### LangChain (Python)

```python
from agent_governance import PolicyEngine, GovernanceCallbackHandler

policy_engine = PolicyEngine.from_yaml("governance-policies.yaml")

agent = create_react_agent(
    llm=llm,
    tools=tools,
    callbacks=[GovernanceCallbackHandler(policy_engine)]
)
```

### CrewAI (Python)

```python
from agent_governance import PolicyEngine
from agent_governance.integrations.crewai import GovernanceTaskDecorator

policy_engine = PolicyEngine.from_yaml("governance-policies.yaml")

@GovernanceTaskDecorator(policy_engine)
def research_task(agent, context):
    return agent.execute(context)
```

### Google ADK (Python)

```python
from agent_governance import PolicyEngine
from agent_governance.integrations.google_adk import GovernancePlugin

policy_engine = PolicyEngine.from_yaml("governance-policies.yaml")

agent = Agent(
    model="gemini-2.0-flash",
    tools=[...],
    plugins=[GovernancePlugin(policy_engine)]
)
```

### TypeScript / Node.js

```ts
import { PolicyEngine } from '@microsoft/agentmesh-sdk';

const policyEngine = PolicyEngine.fromYaml('governance-policies.yaml');

agent.use(policyEngine.middleware());
```

## Next steps

- Explore the toolkit: https://github.com/microsoft/agent-governance-toolkit
- Deploy the sample travel planner repo: https://github.com/Azure-Samples/app-service-agent-otel
- Start with YAML policies, then add Rego/Cedar if you need more complex logic
- Consider Agent Mesh for cross-service/cross-boundary trust

Published: Apr 13, 2026

Version: 1.0


[Read the entire article](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/govern-ai-agents-on-app-service-with-the-microsoft-agent/ba-p/4510962)

