---
external_url: https://devblogs.microsoft.com/semantic-kernel/give-your-agents-domain-expertise-with-agent-skills-in-microsoft-agent-framework/
title: Equip Microsoft Agent Framework Agents with Reusable Agent Skills
author: Sergey Menshykh
primary_section: ai
feed_name: Microsoft Semantic Kernel Blog
date: 2026-03-02 16:46:27 +00:00
tags:
- .NET
- Agent Development
- Agent Framework
- Agent Skills
- AI
- AI Agents
- Automation
- Azure OpenAI
- Context Management
- Domain Expertise
- FileAgentSkillsProvider
- Microsoft Agent Framework
- Microsoft AI
- News
- Python
- Skill Packaging
- SKILL.md
section_names:
- ai
- dotnet
---
Sergey Menshykh explores how Agent Skills in Microsoft Agent Framework empower .NET and Python agents with reusable domain expertise, using FileAgentSkillsProvider and open skill formats.<!--excerpt_end-->

# Equip Microsoft Agent Framework Agents with Reusable Agent Skills

*Author: Sergey Menshykh*

Microsoft Agent Framework now supports **Agent Skills**—modular, portable packages that provide new capabilities and domain expertise to agents built with .NET or Python, without changes to the agents’ core instructions. This release enables developers to add, manage, and discover context-specific skills on demand, unlocking modular and scalable agent design.

## What Are Agent Skills?

Agent Skills use a simple, open format based around a `SKILL.md` file, which includes YAML metadata (like name and description), instructions, and optional scripts or resource files. Agent Skills can cover anything from HR policy compliance to data analysis routines, and are reusable across compatible products and projects.

**Sample skill directory structure:**

```
expense-report/
├── SKILL.md              # Required metadata and instructions
├── scripts/
│   └── validate.py      # Executable support code
├── references/
│   └── POLICY_FAQ.md    # Reference info
└── assets/
    └── expense-report-template.md
```

## Progressive Disclosure

Agent Skills are context efficient: only brief descriptions are loaded at startup, with detailed instructions and resources pulled in as needed. This minimizes token usage for LLM-powered agents, keeping operation cost-effective.

- **Advertise**: Inject skill name/description into agent system prompt
- **Load**: Retrieve `SKILL.md` as needed for user queries
- **Read resources**: On-demand fetch of supplementary files

## Creating a Skill

A skill is a directory with a `SKILL.md` file beginning with YAML frontmatter (minimum: name and description) and detailed instructions.

Example (`meeting-notes/SKILL.md`):

```yaml
---
name: meeting-notes
description: >-
  Summarize meeting transcripts into structured notes with action items.
---

## Instructions

1. Extract key discussion points from the transcript...
```

Supporting directories like `scripts/` or `references/` can be added as the skill expands.

## Integrating Agent Skills

### .NET Example

Install dependencies:

```
dotnet add package Microsoft.Agents.AI --prerelease
```

Code sample:

```csharp
var skillsProvider = new FileAgentSkillsProvider(skillPath: Path.Combine(AppContext.BaseDirectory, "skills"));
AIAgent agent = new AzureOpenAIClient(new Uri(endpoint), new DefaultAzureCredential())
  .GetResponsesClient(deploymentName)
  .AsAIAgent(new ChatClientAgentOptions {
    Name = "SkillsAgent",
    ChatOptions = { Instructions = "You are a helpful assistant." },
    AIContextProviders = [skillsProvider],
  });
AgentResponse response = await agent.RunAsync("Summarize the key points and action items from today's standup meeting.");
```

### Python Example

Install:

```
pip install agent-framework --pre
```

Code sample:

```python
from pathlib import Path
from agent_framework import FileAgentSkillsProvider
from agent_framework.azure import AzureOpenAIChatClient
from azure.identity.aio import AzureCliCredential

skills_provider = FileAgentSkillsProvider(skill_paths=Path(__file__).parent / "skills")
agent = AzureOpenAIChatClient(credential=AzureCliCredential()).as_agent(
    name="SkillsAgent",
    instructions="You are a helpful assistant.",
    context_providers=[skills_provider],
)
response = await agent.run("Summarize the key points and action items from today's standup meeting.")
print(response.text)
```

Once configured, agents automatically discover and load skills that match user requests. Skills can be authored, extended, and combined from multiple directories, supporting multi-team and enterprise workflows.

## Use Cases

- **Enterprise Policy Compliance**: Provide accurate, policy-grounded answers using encapsulated rule skills.
- **Customer Support Playbooks**: Package troubleshooting guidance for consistent agent-driven support.
- **Multi-Team Skill Libraries**: Multiple teams can manage their own skills, merged at runtime by the provider.

## Security Considerations

Treat skills as code dependencies—review for trustworthiness as skill instructions can influence agent behavior.

## Roadmap & Resources

Upcoming features include dynamic, programmatic skill creation and direct script execution support from within skills. Learn more and explore:

- [Agent Skills documentation on Microsoft Learn](https://learn.microsoft.com/en-us/agent-framework/agents/skills)
- [.NET sample](https://github.com/microsoft/agent-framework/tree/main/dotnet/samples/02-agents/AgentSkills)
- [Python sample](https://github.com/microsoft/agent-framework/tree/main/python/samples/02-agents/skills/basic_skill)

This post appeared first on "Microsoft Semantic Kernel Blog". [Read the entire article here](https://devblogs.microsoft.com/semantic-kernel/give-your-agents-domain-expertise-with-agent-skills-in-microsoft-agent-framework/)
