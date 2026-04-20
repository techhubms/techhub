---
author: NaufalPrawironegoro
title: 'Microsoft Fabric Operations Agent: Step-by-step setup and runtime behavior'
primary_section: ai
date: 2026-04-18 07:00:41 +00:00
tags:
- Activator
- Agent Playbook
- AI
- Alerting
- Azure AI Bot Service
- Azure OpenAI Service
- Community
- Cross Geo Processing
- EU Data Boundary
- Eventhouse
- Fabric Admin Portal
- KQL Database
- Kusto Query Language
- Microsoft Fabric
- Microsoft Teams
- ML
- Office 365 Outlook
- Operations Agent
- Pipeline Monitoring
- Power Automate
- Preview Feature
- Real Time Intelligence
- Streaming Data Monitoring
- Tenant Settings
feed_name: Microsoft Tech Community
section_names:
- ai
- ml
external_url: https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/microsoft-fabric-operations-agent-step-by-step-walkthrough/ba-p/4512572
---

NaufalPrawironegoro walks through setting up Microsoft Fabric Operations Agent end-to-end: capacity and Eventhouse prerequisites, enabling the preview in the Admin Portal, wiring a KQL database as a knowledge source, and triggering Power Automate actions via Teams when conditions (like failed pipeline runs) are detected.<!--excerpt_end-->

# Microsoft Fabric Operations Agent: Step-by-step setup and runtime behavior

Microsoft Fabric **Operations Agent** (in the **Real-Time Intelligence** workload) automates continuous monitoring of streaming/telemetry data, surfaces insights when conditions are met, and recommends actions to stakeholders. Instead of relying on someone watching dashboards, the agent:

- Observes your data continuously
- Uses a **large language model (LLM)** to generate a playbook and reason about data
- Notifies people via **Microsoft Teams** when attention is needed

This walkthrough demonstrates a pipeline monitoring scenario: watch for failed pipeline runs and trigger an email alert.

## Prerequisites

### Fabric capacity and workspace

- You need a **Microsoft Fabric workspace backed by a paid capacity**.
  - **Trial capacities are not supported** for Operations Agent.
- Your capacity must be provisioned in a supported region.
  - As of **April 2026**, Operations Agent is available in all Fabric regions **except**:
    - South Central US
    - East US
- If your capacity is outside the **US or EU**, you must enable **cross geo processing and storage for AI** via tenant settings.

### Eventhouse + KQL database

Your workspace must contain:

- An **Eventhouse** (telemetry backbone)
- At least one **KQL database** (tables the agent monitors)

Example workspace used in the guide:

- Workspace: `OperationAgent-WS`
- Eventhouse: `ops_eventhouse`
- KQL databases: `ops_db`, `ops_eventhouse`
- Lakehouse: `ops_lakehouse`

## Enable Operations Agent (preview) in the Admin Portal

A Fabric admin must enable the preview toggle before users can create agents:

1. Open the **Admin Portal**
2. Go to **Real Time Intelligence** settings
3. Find **Enable Operations Agents (Preview)**
4. Set to **Enabled** for:
   - The entire organization, or
   - Specific security groups (for tighter governance)

Also ensure these are enabled at the tenant level:

- **Microsoft Copilot**
- **Azure OpenAI Service** (required to generate the playbook and reason over data)

### Data boundary note

Messages sent to Operations Agents are processed through **Azure AI Bot Service**.

- If your capacity is outside the **EU Data Boundary**, data may be processed outside your geographic or national cloud boundary.
- Communicate this to compliance stakeholders before enabling the feature in production.

## Microsoft Teams requirement

Recipients of agent recommendations must have a **Microsoft Teams** account.

- Install the Teams app: **Fabric Operations Agent** (via Teams app store search)
- The agent sends:
  - Data summaries
  - Recommended actions
  - Approval prompts

## Creating and configuring the Operations Agent

### Step 1: Create a new Operations Agent

1. Open the **Microsoft Fabric portal**
2. Navigate to the target workspace
3. On Fabric home, select the **ellipsis** then **Create**
4. Under **Real Time Intelligence**, select **Operations Agent**
5. Name the agent and select the target workspace

Example used:

- Agent name: `OperationsAgent_1`
- Workspace: `OperationAgent-WS`

### Step 2: Define business goals and agent instructions

On the **Agent Setup** page:

- **Business Goals**: the high-level objective
- **Agent Instructions**: concrete guidance (table, columns, alert condition)

Example:

- Business goal:
  - *“Monitor data pipeline execution and alert on failures.”*
- Agent instructions:
  - *“Monitor pipeline_runs table. Alert when status is failed.”*

After saving, Fabric generates an **Agent Playbook** (LLM-generated). The playbook can include:

- **Business Term Glossary** mapping inferred objects to your data
  - Example inferred object: `PipelineRun`
    - Mapped to `pipeline_runs`
    - Properties:
      - `status` (from `status` column)
      - `runId` (from `run_id` column)
- **Rules** the agent will evaluate

Review the playbook carefully—LLM output can misinterpret:

- Validate each property-to-column mapping
- Confirm rules match your intended thresholds
- If incorrect, adjust goals/instructions and regenerate

### Step 3: Add a knowledge source

In the **Knowledge** section:

1. Select **Add Data**
2. Choose from accessible **KQL databases** and **Eventhouses**
3. Pick the source that contains the table referenced in your instructions

Example:

- Select `ops_db` because it contains `pipeline_runs`

The agent queries the knowledge source on a schedule (about **every five minutes**) to evaluate rules.

### Step 4: Define actions

Actions are what the agent can recommend when a rule matches.

1. In **Actions**, select **Add Action**
2. Create a **New Custom Action** with:
   - **Action Name**
   - **Action Description**
   - Optional **Parameters** (dynamic values passed into the flow)

Example action:

- Name: `Send Email Alert`
- Description: send an email notification when a pipeline failure is detected

### Step 5: Configure the custom action with Power Automate

To connect the action to automation:

1. Open the action’s **Configure Custom Action** pane
2. Select the **Workspace** where the activator item exists
3. Select the **Activator** (bridges Operations Agent and Power Automate)
   - Example: `Email_Alert_Activator`
4. Copy the generated **Connection String** (this links the agent to the flow)
5. Select **Open Flow Builder**

### Step 6: Build the Power Automate flow

In Power Automate:

- The flow is pre-configured with the trigger:
  - **When an Activator Rule is Triggered**

In the trigger’s **Parameters**:

- Paste the **Connection String**
  - If missing/incorrect, the flow won’t fire when the agent recommends the action

Then add steps such as:

- An **Office 365 Outlook** action to send email
- Use dynamic content (for example):
  - pipeline run ID
  - failure status
  - any parameters from the Operations Agent

Save the flow and return to Fabric.

### Step 7: Generate the playbook and start the agent

1. Select **Save** on the Agent Setup page to generate/update the playbook
2. Optionally use **Generate Playbook** to regenerate after edits
3. Select **Start** to begin monitoring

Operational details:

- The agent queries the knowledge source about **every five minutes**
- When conditions match, it uses the LLM to:
  - Summarize the data
  - Generate a recommendation
  - Notify recipients via Teams

You can select **Stop** at any time (useful for demos).

## Runtime behavior (how it works once started)

The Operations Agent runs in a loop:

1. Query the connected KQL database every ~5 minutes
2. Evaluate playbook rules
3. If a condition matches (example: `pipeline_runs.status == "failed"`):
   - The LLM analyzes and summarizes the triggering data
   - The agent sends a Teams message with:
     - summary of the insight
     - data context
     - suggested action
   - Recipient can **approve (Yes)** or **reject (No)** the action
   - Parameters (run ID, severity, etc.) can be reviewed/adjusted before approval
4. If approved, the agent executes the action using the **creator’s credentials**
   - In this demo: triggers a Power Automate flow that sends an email alert

Timeout behavior:

- If a recommendation is not responded to within **three days**, it is automatically canceled and can’t be approved afterward.


[Read the entire article](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/microsoft-fabric-operations-agent-step-by-step-walkthrough/ba-p/4512572)

