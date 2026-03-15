---
external_url: https://devblogs.microsoft.com/agent-framework/give-your-agents-domain-expertise-with-agent-skills-in-microsoft-agent-framework/
title: Give Your Agents Domain Expertise with Agent Skills in Microsoft Agent Framework
author: Sergey Menshykh
primary_section: ai
feed_name: Microsoft Semantic Kernel Blog
date: 2026-03-02 16:46:27 +00:00
tags:
- .NET
- Agent Framework
- Agent Skills
- AI
- AI Agents
- Azure OpenAI
- Chatbot Development
- Context Management
- Customer Support Automation
- Enterprise Policy Compliance
- Microsoft Agent Framework
- News
- Package Management
- Productivity Tools
- Python
- Reusable Skills
- Skill Packages
- Skills Provider
section_names:
- ai
- dotnet
---
Sergey Menshykh explains how Agent Skills enhance Microsoft Agent Framework agents by enabling reusable, dynamic domain expertise through portable skill packages, with implementation examples for .NET and Python.<!--excerpt_end-->

# Give Your Agents Domain Expertise with Agent Skills in Microsoft Agent Framework

Agent Skills are now available for the Microsoft Agent Framework, allowing developers to create portable, reusable skill packages that can extend an agent’s expertise on demand. With built-in skill providers for .NET and Python, agents can discover and utilize Agent Skills at runtime without modifying their core instructions.

## What Are Agent Skills?

Agent Skills provide a standardized way to package domain knowledge (like HR policy, expense management, or customer support playbooks) as modular, pluggable components. Each skill is a folder containing a `SKILL.md` file describing what the skill does and how to perform its tasks, plus optional scripts and reference files for richer context or executable automation.

**Skill Structure Example:**

```
expense-report/
  ├── SKILL.md
  ├── scripts/
  │   └── validate.py
  ├── references/
  │   └── POLICY_FAQ.md
  └── assets/
      └── expense-report-template.md
```

The `SKILL.md` file uses YAML frontmatter for metadata (e.g., name, description, compatibility), followed by detailed markdown instructions. Only `name` and `description` are required fields.

**Sample YAML Frontmatter:**

```yaml
---
name: expense-report
description: >-
  File and validate employee expense reports according to company policy.
license: Apache-2.0
compatibility: Requires python3
metadata:
  author: contoso-finance
  version: "2.1"
---
```

## Progressive Disclosure for Efficient Context Management

Agent Skills operate with a progressive disclosure approach:

1. **Advertise:** Skill names and descriptions briefly inform the agent of capabilities.
2. **Load:** Full instructions are loaded only when needed (matching a user task).
3. **Read resources:** Supplementary docs, scripts, or templates are fetched as required.

This model minimizes token usage and keeps the agent efficient while supporting rich expertise.

## Creating and Managing Skills

To add a new skill, simply create a folder with a `SKILL.md` document and optionally add code or resource files. The description field is critical to help the agent decide when to load the skill.

**Minimal Skill Example:**

```
skills/
  └── meeting-notes/
      └── SKILL.md
```

**Sample SKILL.md Frontmatter:**

```yaml
---
name: meeting-notes
description: >-
  Summarize meeting transcripts with action items. Use when asked to process meeting recordings or transcripts.
---
```

## Integrating Skills with the Agent Framework

### In .NET

Install required packages:

```bash
dotnet add package Microsoft.Agents.AI --prerelease
dotnet add package Microsoft.Agents.AI.OpenAI --prerelease
dotnet add package Azure.AI.OpenAI --prerelease
dotnet add package Azure.Identity
```

Set up the skills provider:

```csharp
var skillsProvider = new FileAgentSkillsProvider(skillPath: Path.Combine(AppContext.BaseDirectory, "skills"));
AIAgent agent = new AzureOpenAIClient(new Uri(endpoint), new DefaultAzureCredential())
    .GetResponsesClient(deploymentName)
    .AsAIAgent(new ChatClientAgentOptions {
        Name = "SkillsAgent",
        ChatOptions = new() { Instructions = "You are a helpful assistant." },
        AIContextProviders = [skillsProvider],
    });
AgentResponse response = await agent.RunAsync("Summarize the key points and action items from today's standup meeting.");
Console.WriteLine(response.Text);
```

### In Python

Install with pip:

```bash
pip install agent-framework --pre
```

Set up the skills provider:

```python
from pathlib import Path
from agent_framework import SkillsProvider
from agent_framework.azure import AzureOpenAIChatClient
from azure.identity.aio import AzureCliCredential
skills_provider = SkillsProvider(skill_paths=Path(__file__).parent / "skills")
agent = AzureOpenAIChatClient(credential=AzureCliCredential()).as_agent(
    name="SkillsAgent",
    instructions="You are a helpful assistant.",
    context_providers=[skills_provider],
)
response = await agent.run("Summarize the key points and action items from today's standup meeting.")
print(response.text)
```

No manual routing logic is needed—the agent dynamically loads relevant skills when appropriate.

## Practical Use Cases

- **Enterprise Policy Compliance:** Encode HR or IT policy as skills, ensuring consistent, up-to-date answers.
- **Customer Support Playbooks:** Package support procedures for automated, repeatable troubleshooting.
- **Multi-Team Skill Libraries:** Teams can develop and maintain independent libraries that the agent can aggregate.

## Security Considerations

Treat skill packages with the same caution as open-source dependencies—review code and documentation from trusted sources before integrating into your agents. Skills can influence agent behavior and context.

## Roadmap and Further Resources

Future updates to Agent Skills will include programmatic skill registration and execution of embedded scripts for more dynamic and interactive experiences.

- [Agent Skills documentation on Microsoft Learn](https://learn.microsoft.com/en-us/agent-framework/agents/skills)
- [.NET Agent Skills sample](https://github.com/microsoft/agent-framework/tree/main/dotnet/samples/02-agents/AgentSkills)
- [Python Agent Skills sample](https://github.com/microsoft/agent-framework/tree/main/python/samples/02-agents/skills/basic_skill)

This post appeared first on "Microsoft Semantic Kernel Blog". [Read the entire article here](https://devblogs.microsoft.com/agent-framework/give-your-agents-domain-expertise-with-agent-skills-in-microsoft-agent-framework/)
