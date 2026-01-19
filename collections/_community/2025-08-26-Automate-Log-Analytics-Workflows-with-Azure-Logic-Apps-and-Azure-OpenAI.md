---
external_url: https://techcommunity.microsoft.com/t5/azure-observability-blog/automate-your-log-analytics-workflows-with-ai-and-logic-apps/ba-p/4442803
title: Automate Log Analytics Workflows with Azure Logic Apps and Azure OpenAI
author: NoyaBlanga
viewing_mode: external
feed_name: Microsoft Tech Community
date: 2025-08-26 13:19:57 +00:00
tags:
- Application Insights
- Automation
- Azure Logic Apps
- Azure Monitor
- Azure OpenAI
- Cloud Monitoring
- Data Summarization
- Email Reports
- Error Analysis
- GPT 4
- KQL
- LLMs
- Log Analytics
- Microsoft Azure
- Operational Insights
- Workflows
section_names:
- ai
- azure
---
NoyaBlanga explains how to automate log analysis workflows using Azure Logic Apps, Log Analytics, and Azure OpenAI, enabling developers to deliver actionable, AI-driven insights via scheduled workflows.<!--excerpt_end-->

# Automate Your Log Analytics Workflows with AI and Logic Apps

Maintaining clear visibility into system health relies on timely log analytics. This guide walks you through automating those insights using Azure Logic Apps, Log Analytics queries, and large language models (LLMs) through Azure OpenAI. With this approach, you can save time, reduce manual analysis, and empower your team with consistent, AI-driven insights.

## Why Automate Log Analysis?

Logs offer valuable stories about what’s happening in your environment. However, manually reviewing logs and surfacing key findings is time-consuming. Automation using Microsoft technologies enables:

- Scheduled collection and querying of logs from Application Insights and Log Analytics
- Automated summarization using LLMs (e.g., GPT-4 via Azure OpenAI)
- Direct delivery of actionable, email-ready reports

## Example Scenario: Application Insights

Suppose you run critical services and want to track error and critical event trends hour-by-hour. By querying AppTraces in Application Insights and leveraging AI summarization, you streamline:

- Spotting spikes and drops
- Surfacing recurring error patterns
- Identifying top problem operations

## Step-by-Step Workflow

### 1. Create the Logic App

- In the Azure Portal, create a Logic App and open the Designer.
- Reference: [Overview - Azure Logic Apps | Microsoft Learn](https://learn.microsoft.com/en-us/azure/logic-apps/logic-apps-overview)

### 2. Schedule the Trigger

- Use the **Recurrence** trigger to run your workflow on a regular schedule (e.g., weekly).

### 3. Query Log Analytics Data

- Add the **Azure Monitor Logs — Run query and list results** connector.
- Connect to your Log Analytics workspace.
- Write a Kusto Query Language (KQL) statement, such as:

  ```kusto
  AppTraces
    | where TimeGenerated > startofday(ago(7d))
    | where SeverityLevel in (3, 4) // Error = 3, Critical = 4
    | summarize TracesCount = count(), SampleMessages = make_list(Message, 3) by bin(TimeGenerated, 1h), SeverityLevel, OperationName
    | order by TimeGenerated asc
  ```

- *Tip:* Use `summarize` to aggregate data and create smaller, more informative results for processing.
- Reference: [Connect to Log Analytics or Application Insights - Azure Logic Apps](https://learn.microsoft.com/en-us/azure/connectors/connectors-azure-monitor-logs?tabs=standard)

### 4. Prerequisite: Configure Azure OpenAI

- Set up an Azure OpenAI resource with your preferred model (e.g., GPT-4).
- Reference: [What is Azure OpenAI in Azure AI Foundry Models? | Microsoft Learn](https://learn.microsoft.com/en-us/azure/ai-foundry/openai/overview)

### 5. Analyze and Summarize with Azure OpenAI

- Add an **HTTP** action to call your Azure OpenAI API endpoint.
- Pass KQL query results as a prompt, with structured instructions for summarization:
  - Aggregate error and critical counts
  - Identify affected operations and major trends
  - Highlight spikes, recurring issues, and recommend urgent actions
  - Format as HTML for direct email use
- *Prompt Example:* See Microsoft Learn [How to use Assistants with Logic apps - Azure OpenAI](https://learn.microsoft.com/en-us/azure/ai-foundry/openai/how-to/assistants-logic-apps)

### 6. Send the Report via Email or Teams

- Use the **Send an email (V2)** connector, or route to Teams.
- Tailor the email subject and importance, then send the AI-generated report directly to your team.

### Final Steps

- Enable your Logic App so that scheduled executions run as intended.
- Review delivered reports to keep stakeholders informed and proactive.

## Key Takeaways

By combining Azure Logic Apps, Log Analytics, and Azure OpenAI, you can transform extensive logs into structured, actionable summaries, minimizing manual effort and accelerating incident response.

---

**Ready to implement this in your own Microsoft environment? Begin building automated AI-driven log analytics workflows today!**

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-observability-blog/automate-your-log-analytics-workflows-with-ai-and-logic-apps/ba-p/4442803)
