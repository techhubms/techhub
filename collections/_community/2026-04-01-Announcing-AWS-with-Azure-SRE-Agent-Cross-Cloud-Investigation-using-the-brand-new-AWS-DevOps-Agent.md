---
primary_section: azure
feed_name: Microsoft Tech Community
date: 2026-04-01 21:49:12 +00:00
title: 'Announcing AWS with Azure SRE Agent: Cross-Cloud Investigation using the brand new AWS DevOps Agent'
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/announcing-aws-with-azure-sre-agent-cross-cloud-investigation/ba-p/4507413
tags:
- Agent SOPs
- AWS Access Keys
- AWS DevOps Agent
- AWS IAM
- AWS MCP Server
- AWS Regional Endpoints
- Azure
- Azure Portal
- Azure SRE Agent
- CloudTrail
- CloudWatch
- Community
- Cross Cloud Operations
- DevOps
- IAM Policy
- Incident Investigation
- Least Privilege
- MCP
- MCP Server
- Operational Workflows
- SigV4 Signing
- Stdio Transport
- Uv
- Uvx
author: dbandaru
section_names:
- azure
- devops
---

dbandaru walks through connecting Azure SRE Agent to the AWS MCP Server so you can run AWS API calls, use guided operational workflows, and start AWS DevOps Agent investigations from the same Azure SRE Agent chat session.<!--excerpt_end-->

## Overview

This guide shows how to connect **Azure SRE Agent** to AWS using the official **AWS MCP server** and **MCP Proxy for AWS**:

- Query AWS documentation
- Execute authenticated calls across **15,000+ AWS APIs**
- Run pre-built operational workflows (**Agent SOPs**)
- Start incident investigations through **AWS DevOps Agent** (GA)

Key links:

- MCP Proxy for AWS (GitHub): https://github.com/aws/mcp-proxy-for-aws
- AWS DevOps Agent GA announcement: https://aws.amazon.com/blogs/mt/announcing-general-availability-of-aws-devops-agent/

## How it works

- The **MCP Proxy for AWS** runs as a **local stdio process** that Azure SRE Agent spawns via `uvx`.
- AWS authentication is handled via AWS credentials you provide as environment variables.
- No container deployment or separate infrastructure is required.
- In the Azure portal, you connect it using the generic **MCP server (User provided connector)** option with **stdio** transport.

When connected, the AWS MCP Server exposes **23 MCP tools** grouped into:

- Documentation and knowledge
- API execution
- Guided workflows
- DevOps Agent operations

## Key capabilities

| Area | Capabilities |
| --- | --- |
| Documentation | Search AWS docs, API references, and best practices; retrieve pages as markdown |
| API execution | Execute authenticated calls across 15,000+ AWS APIs with syntax validation and error handling |
| Agent SOPs | Pre-built multi-step workflows following AWS Well-Architected principles |
| Regional info | List regions; check service/feature availability by region |
| Infrastructure | Provision VPCs, databases, compute, storage, networking |
| Troubleshooting | Analyze CloudWatch logs, CloudTrail events, permissions, app failures |
| Cost management | Billing alerts, usage analysis, cost data review |
| DevOps Agent | Start investigations, read RCAs, get remediation recommendations, chat |

Note:

- The AWS MCP Server is free to use; you pay only for AWS resources consumed.
- All actions respect your existing IAM policies.

## Prerequisites

- An **Azure SRE Agent** resource deployed in Azure
- An AWS account with IAM credentials configured
- The **uv** package manager installed on the SRE Agent host (used to run the proxy via `uvx`): https://docs.astral.sh/uv/
- IAM permissions for the MCP server:
  - `aws-mcp:InvokeMcp`
  - `aws-mcp:CallReadOnlyTool`
  - Optional: `aws-mcp:CallReadWriteTool`

## Step 1: Create AWS access keys

The AWS MCP server authenticates using **AWS access keys** (Access Key ID + Secret Access Key) tied to an IAM user.

### Navigate to IAM

1. Sign in to the AWS Management Console: https://console.aws.amazon.com/
2. Open IAM: `https://console.aws.amazon.com/iam/`
3. Go to Users: `https://console.aws.amazon.com/iam/home#/users`

### Create a dedicated IAM user

Create a dedicated user (rather than using a personal account) so you can scope permissions and rotate keys independently.

1. Select **Create user**
2. Use a name like `sre-agent-mcp`
3. Do not enable AWS Console access (programmatic access only)
4. Attach a policy you create with the following JSON:

```json
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Action": [
        "aws-mcp:InvokeMcp",
        "aws-mcp:CallReadOnlyTool",
        "aws-mcp:CallReadWriteTool"
      ],
      "Resource": "*"
    }
  ]
}
```

Tip: For production, you still need to add the service-specific IAM permissions required by the actual AWS APIs you call (for example `ec2:DescribeInstances`, `logs:GetQueryResults`). Start broad for testing, then scope down (least privilege).

### Generate access keys

1. Open the new user
2. Go to **Security credentials**
3. Create an access key for the use case **Third-party service**
4. Copy both values immediately (the Secret Access Key is shown only once)

You’ll use them as connector environment variables:

- `AWS_ACCESS_KEY_ID`
- `AWS_SECRET_ACCESS_KEY`

### Required permissions summary

| Permission | Description | Required? |
| --- | --- | --- |
| `aws-mcp:InvokeMcp` | Base access to the AWS MCP server | Yes |
| `aws-mcp:CallReadOnlyTool` | Read operations (describe/list/get/search) | Yes |
| `aws-mcp:CallReadWriteTool` | Write operations (create/update/delete) | Optional |

## Step 2: Add the MCP connector in Azure

The proxy runs locally via `uvx` and forwards requests to the AWS MCP endpoint, handling **SigV4 signing** using your credentials.

### Determine your AWS MCP endpoint

| AWS Region | MCP Endpoint URL |
| --- | --- |
| us-east-1 (default) | `https://aws-mcp.us-east-1.api.aws/mcp` |
| us-west-2 | `https://aws-mcp.us-west-2.api.aws/mcp` |
| eu-west-1 | `https://aws-mcp.eu-west-1.api.aws/mcp` |

Without the `--metadata AWS_REGION=<region>` argument, operations default to `us-east-1`.

### Configure in the Azure portal

1. In Azure portal, open your **SRE Agent** resource
2. Go to **Builder** > **Connectors**
3. Select **Add connector**
4. Choose **MCP server (User provided connector)**
5. Use these settings:

| Field | Value |
| --- | --- |
| Name | `aws-mcp` |
| Connection type | stdio |
| Command | `uvx` |
| Arguments | `mcp-proxy-for-aws@latest https://aws-mcp.us-east-1.api.aws/mcp --metadata AWS_REGION=us-west-2` |
| Environment variables | `AWS_ACCESS_KEY_ID=<your-access-key-id>`, `AWS_SECRET_ACCESS_KEY=<your-secret-access-key>` |

Equivalent MCP client configuration (example):

```json
{
  "mcpServers": {
    "aws-mcp": {
      "command": "uvx",
      "args": [
        "mcp-proxy-for-aws@latest",
        "https://aws-mcp.us-east-1.api.aws/mcp",
        "--metadata",
        "AWS_REGION=us-west-2"
      ]
    }
  }
}
```

Security notes:

- Store credentials securely; connector environment variables are stored encrypted in the portal.
- Don’t commit credentials to source control.
- If the host already has AWS credentials configured (e.g., `aws configure` or an instance profile), the proxy can pick them up automatically.

## Step 3: Add an AWS skill

Skills provide domain knowledge and instructions so the agent can use the MCP tools effectively.

The post recommends **skills over subagents** when you want to keep conversation context intact and avoid handoff latency; use subagents when you need isolation and stricter tool restrictions.

### Skill configuration (as provided)

1. In Azure portal, go to **Builder** > **Skills**
2. Select **Add skill**
3. Paste the provided `SkillConfiguration` YAML (includes overview, tool usage guidance, and troubleshooting)
4. Ensure it references the connector:

```yaml
mcp_connectors:
- aws-mcp
```

## Step 4: Test the integration

Start a new chat with Azure SRE Agent and try:

```text
What AWS regions are available?
```

If authentication fails, revisit the IAM credentials and permissions.

Example prompts from the guide:

- Documentation:

```text
Search AWS documentation for EKS best practices for production clusters
What AWS regions support Amazon Bedrock?
Read the AWS documentation page about S3 bucket policies
```

- Infrastructure queries:

```text
List all my running EC2 instances in us-east-1
Show me the details of my EKS cluster named "production-cluster"
What Lambda functions are deployed in my account?
```

- Monitoring/troubleshooting:

```text
What CloudWatch alarms are currently in ALARM state?
Search CloudWatch Logs for errors in the /aws/lambda/my-function log group
My Lambda function is timing out. Walk me through the investigation.
```

- Cross-cloud scenarios:

```text
My Azure Function is failing when calling AWS S3. Check if there are any S3 service issues and review the bucket policy for "my-data-bucket".
Compare the health of my AWS EKS cluster with my Azure AKS cluster.
```

## What's new: AWS DevOps Agent integration

The AWS MCP server includes tools for **AWS DevOps Agent**:

- AgentSpace management: `aws___list_agent_spaces`, `aws___get_agent_space`, `aws___create_agent_space`
- Investigation lifecycle: `aws___create_investigation`, `aws___get_task`, `aws___list_tasks`, `aws___list_journal_records`, `aws___list_executions`, `aws___list_recommendations`, `aws___get_recommendation`
- Proactive evaluations: `aws___start_evaluation`, `aws___list_goals`
- Real-time chat: `aws___create_chat`, `aws___list_chats`, `aws___send_message`

## Cross-agent investigation workflow

The described workflow:

1. Start an AWS DevOps Agent investigation for AWS-side symptoms
2. Investigate Azure in parallel (Azure Monitor, Log Analytics, resource health)
3. Read AWS results (journal + recommendations)
4. Correlate both clouds into a single RCA and remediation plan

## Architecture (high level)

The integration is a stdio proxy chain:

```text
Azure SRE Agent
  | stdio (local process)
  v
mcp-proxy-for-aws (spawned via uvx)
  | authenticated HTTPS
  v
AWS MCP Server (aws-mcp.<region>.api.aws)
  |-- AWS API calls -> AWS services (EC2, S3, CloudWatch, EKS, Lambda, ...)
  '-- DevOps Agent API calls -> AWS DevOps Agent (AgentSpaces, Investigations, Recommendations, Chat)
```

## Troubleshooting

Common issues and fixes from the guide:

- `403 Forbidden`: IAM user missing `aws-mcp:InvokeMcp` / `aws-mcp:CallReadOnlyTool`
- `401 Unauthorized`: rotate keys and update connector environment variables
- Proxy won’t start: install `uv` / ensure `uvx` is on PATH
- Timeout: allow outbound HTTPS (443) to `aws-mcp.<region>.api.aws`
- Tools not available after adding connector: MCP connections initialize at agent startup (restart/redeploy)
- First run is slow: `uvx` downloads dependencies (wait up to ~30 seconds)

## Related content

- AWS MCP Server docs: https://docs.aws.amazon.com/aws-mcp/latest/userguide/what-is-mcp-server.html
- MCP Proxy for AWS: https://github.com/aws/mcp-proxy-for-aws
- Tools reference: https://docs.aws.amazon.com/aws-mcp/latest/userguide/understanding-mcp-server-tools.html
- AWS DevOps Agent docs: https://docs.aws.amazon.com/devopsagent/latest/userguide/
- AWS DevOps Agent GA announcement: https://aws.amazon.com/blogs/mt/announcing-general-availability-of-aws-devops-agent/
- AWS IAM docs: https://docs.aws.amazon.com/IAM/latest/UserGuide/


[Read the entire article](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/announcing-aws-with-azure-sre-agent-cross-cloud-investigation/ba-p/4507413)

