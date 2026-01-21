---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/agentic-applications-on-azure-container-apps-with-microsoft/ba-p/4467601
title: Deploying Agentic Applications on Azure Container Apps with Microsoft Foundry
author: Cary_Chai
feed_name: Microsoft Tech Community
date: 2025-11-18 20:30:56 +00:00
tags:
- AI Agent
- Application Insights
- Automated Deployment
- Azure Container Apps
- Azure Developer CLI
- Azure Monitor
- Cloud Native
- Distributed Tracing
- FastAPI
- GenAI
- MAF
- Microservices
- Microsoft Agent Framework
- Microsoft Foundry
- Observability
- OpenTelemetry
- Serverless
- Telemetry
section_names:
- ai
- azure
---
Cary_Chai details how to deploy and observe agentic applications on Azure Container Apps by integrating the Microsoft Agent Framework and Microsoft Foundry, with full telemetry and monitoring support.<!--excerpt_end-->

# Deploying Agentic Applications on Azure Container Apps with Microsoft Foundry

Agentic applications are reshaping modern architectures, enabling developers to build complex, intelligent systems that automate workflows, invoke tools, and orchestrate multi-step processes. This guide shows how to deploy such applications using Azure Container Apps (ACA), enhanced by the Microsoft Agent Framework (MAF), and observability through Microsoft Foundry and OpenTelemetry.

## Why Azure Container Apps + MAF + Foundry?

Azure Container Apps offers a fully managed, serverless platform to run microservices and composable workloads with autoscaling and integrated communication. When you combine it with MAF—which structures agent logic and emits rich telemetry—and Microsoft Foundry, you get:

- Containerized, scalable agent execution
- Centralized observability, reasoning graphs, and tool tracking
- Monitoring, debugging, and performance insights via Foundry’s dashboards

## Prerequisites

- Active Azure subscription ([create free account](https://azure.microsoft.com/free/))
- Microsoft Foundry project ([setup here](https://ai.azure.com/projects))
- [Azure Developer CLI (azd)](https://learn.microsoft.com/azure/developer/azure-developer-cli/install-azd) installed
- [Git](https://git-scm.com/downloads) installed

## Deploying the Sample Agent

Sample code: [foundry-3p-agents-samples](https://github.com/cachai2/foundry-3p-agents-samples)

This solution deploys:

- An Azure Container Apps environment
- An agent with Microsoft Agent Framework (MAF)
- OpenTelemetry instrumentation, exporting to Application Insights
- Application Insights for telemetry
- Microsoft Foundry resource, wired for integration

### Deployment Steps

1. **Clone the repository:**

   ```sh
   git clone https://github.com/cachai2/foundry-3p-agents-samples.git
   cd foundry-3p-agents-samples/azure
   ```

2. **Authenticate with Azure:**

   ```sh
   azd auth login
   ```

3. **Set the Azure AI model environment variable:**

   ```sh
   azd env set AZURE_AI_MODEL_DEPLOYMENT_NAME gpt-4.1-mini
   ```

4. **Deploy resources:**

   ```sh
   azd up
   ```

   This provisions ACA, Application Insights, and configures your environment.

## Integrating Observability

To connect your agent to Foundry, you must:

- Enable MAF to emit standardized telemetry (gen_ai.* spans)
- Configure OpenTelemetry and Azure Monitor exporter

### Microsoft Agent Framework (MAF) Configuration

- Built-in tools (example: `get_exchange_rate`)
- Utilizes FastAPI to expose endpoints (e.g., `/invoke`)
- Telemetry setup with `configure_azure_monitor` and `setup_observability()` to emit all required traces

#### Example Setup Snippet

```python
from azure.monitor.opentelemetry import configure_azure_monitor
from agent_framework.observability import setup_observability
from opentelemetry.sdk.resources import Resource

SERVICE_NAME = os.getenv("ACA_SERVICE_NAME", "aca-currency-exchange-agent")
configure_azure_monitor(
    resource=Resource.create({"service.name": SERVICE_NAME}),
    connection_string=os.getenv("APPLICATION_INSIGHTS_CONNECTION_STRING"),
)
setup_observability(enable_sensitive_data=False)
```

After deployment, all telemetry is sent to Application Insights and is ready for Foundry ingestion.

## Testing and Verification

1. **Add your model in Foundry:** Open [Microsoft Foundry](https://ai.azure.com/), go to Model Catalog, and add `gpt-4.1-mini`.
2. **Get your app URL from ACA.**
3. **Invoke the agent:**

   ```sh
   export APP_URL="your ACA app URL"
   curl -X POST "$APP_URL/invoke" \
        -H "Content-Type: application/json" \
        -d '{ "prompt": "How do I convert 100 USD to EUR?" }'
   ```

4. **Check Application Insights telemetry.**
   Use KQL queries in Logs to validate that spans like `gen_ai.model.*`, tool calls, and agent lifecycle events are flowing.

## Connecting Application Insights to Foundry

1. Go to Monitoring > Application analytics in Foundry.
2. Select your Application Insights resource created by `azd up` and connect it.
3. If this fails, use Foundry's Management Center to manually connect Application Insights.

Once connected, Foundry visualizes:

- Agent monitoring metrics
- Tool call traces
- Reasoning graphs and orchestration flows
- Error and success rates

## Monitoring in Foundry

- **Monitoring Tab:** Get high-level metrics (inference calls, success/error rates, latency, token usage)
- **Tracing Tab:** Drill down into distributed traces for each agent request—see tool invocations, internal steps, and token usage

## Conclusion

Combining Azure Container Apps, Microsoft Agent Framework, and Microsoft Foundry enables you to build, deploy, and monitor intelligent, scalable agentic applications in the Microsoft cloud. With fully automated observability and modern deployment workflows, this architecture lays the foundation for next-generation AI-powered microservices.

---
Author: Cary_Chai

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/agentic-applications-on-azure-container-apps-with-microsoft/ba-p/4467601)
