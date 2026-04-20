---
author: Sergey Menshykh
date: 2026-04-13 09:49:20 +00:00
feed_name: Microsoft Semantic Kernel Blog
section_names:
- ai
- azure
- dotnet
tags:
- .NET
- Agent Framework
- Agent Skills
- AgentClassSkill
- AgentInlineSkill
- AgentSkillsProvider
- AgentSkillsProviderBuilder
- AI
- Azure
- Azure OpenAI
- Azure.AI.OpenAI
- DefaultAzureCredential
- Dependency Injection
- File Based Skills
- Human in The Loop
- Microsoft Agent Framework
- News
- NuGet Package
- Reflection
- Script Execution
- Skill Resources
- Skill Scripts
- Tool Approval
external_url: https://devblogs.microsoft.com/agent-framework/agent-skills-in-net-three-ways-to-author-one-provider-to-run-them/
title: 'Agent Skills in .NET: Three Ways to Author, One Provider to Run Them'
primary_section: ai
---

Sergey Menshykh explains how Agent Skills in .NET can be authored as file-based skills, inline C# skills, or class-based skills (for example shipped via NuGet), then composed into a single provider. The article also shows script execution and a human-approval gate for tool/script calls in production-style agent scenarios.<!--excerpt_end-->

## Overview

Agent Skills in .NET support three ways to author and deliver skills, all composable through a single provider:

- **File-based skills** stored on disk (a skill folder with `SKILL.md`, scripts, and reference docs)
- **Class-based skills** packaged as .NET classes (often shipped via **NuGet**)
- **Inline code-defined skills** authored directly in application code

The provider can also:

- Run scripts via a script runner (example: `SubprocessScriptRunner.RunAsync`)
- Require **human approval** before scripts run (`UseScriptApproval(true)`) for safer automation

## The scenario

You’re building an **HR self-service agent**. It evolves over time:

1. Start with a file-based onboarding skill.
2. Add a benefits enrollment skill shipped by another team as a NuGet package.
3. Bridge a missing capability (time-off balance) with an inline skill until the NuGet version ships.

Each change is self-contained and doesn’t require rewriting the others.

## Step 1: A file-based skill with script execution

A file-based skill is represented as a directory containing `SKILL.md`, scripts, and reference material:

```text
skills/
└── onboarding-guide/
    ├── SKILL.md
    ├── scripts/
    │   └── check-provisioning.py
    └── references/
        └── onboarding-checklist.md
```

Example `SKILL.md` frontmatter and instructions:

```yaml
---
name: onboarding-guide
description: >-
  Walk new hires through their first-week setup checklist. Use when a new employee asks about system access, required training, or onboarding steps.
---

## Instructions

1. Ask for the employee's name and start date if not already provided.
2. Run the `scripts/check-provisioning.py` script to verify their IT accounts are active.
3. Walk through the steps in the `references/onboarding-checklist.md` reference.
4. Follow up on any incomplete items.
```

Wire skills + script execution into an agent using `AgentSkillsProvider` and a script runner:

```csharp
using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Agents.AI;
using OpenAI.Responses;

string endpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT")!;
string deploymentName = Environment.GetEnvironmentVariable("AZURE_OPENAI_DEPLOYMENT_NAME") ?? "gpt-4o-mini";

var skillsProvider = new AgentSkillsProvider(
    Path.Combine(AppContext.BaseDirectory, "skills"),
    SubprocessScriptRunner.RunAsync);

AIAgent agent = new AzureOpenAIClient(new Uri(endpoint), new DefaultAzureCredential())
    .GetResponsesClient()
    .AsAIAgent(
        new ChatClientAgentOptions
        {
            Name = "HRAgent",
            ChatOptions = new()
            {
                Instructions = "You are a helpful HR self-service assistant."
            },
            AIContextProviders = [skillsProvider],
        },
        model: deploymentName);

AgentResponse response = await agent.RunAsync(
    "I just started here. What are the onboarding steps I need to follow?");

Console.WriteLine(response.Text);
```

Notes on script execution:

- `SubprocessScriptRunner` executes scripts by receiving the skill, script, and arguments.
- For production, the article recommends a runner that adds **sandboxing**, **resource limits**, **input validation**, and **audit logging**.

## Step 2: Adding a class-based skill from NuGet

A team can publish a skill as a NuGet package (example: `Contoso.Skills.HrEnrollment`). Class-based skills:

- Derive from `AgentClassSkill<T>`
- Declare resources and scripts through attributes so the framework discovers them via reflection:
  - `[AgentSkillResource]`
  - `[AgentSkillScript]`

Example skill inside the package:

```csharp
// Inside the Contoso.Skills.HrEnrollment NuGet package
using System.ComponentModel;
using System.Text.Json;
using Microsoft.Agents.AI;

public sealed class BenefitsEnrollmentSkill : AgentClassSkill<BenefitsEnrollmentSkill>
{
    public override AgentSkillFrontmatter Frontmatter { get; } = new(
        "benefits-enrollment",
        "Enroll an employee in health, dental, or vision plans. Use when asked about benefits sign-up, plan options, or coverage changes.");

    protected override string Instructions => """
    Use this skill when an employee asks about enrolling in or changing their benefits.
    1. Read the available-plans resource to review current offerings and pricing.
    2. Confirm the plan the employee wants to enroll in.
    3. Use the enroll script to complete the enrollment.
    """;

    [AgentSkillResource("available-plans")]
    [Description("Health, dental, and vision plan options with monthly pricing.")]
    public string AvailablePlans => """
    ## Available Plans (2026)
    - Health: Basic HMO ($0/month), Premium PPO ($45/month)
    - Dental: Standard ($12/month), Enhanced ($25/month)
    - Vision: Basic ($8/month)
    """;

    [AgentSkillScript("enroll")]
    [Description("Enrolls an employee in the specified benefit plan. Returns a JSON confirmation.")]
    private static string Enroll(string employeeId, string planCode)
    {
        bool success = HrClient.EnrollInPlan(employeeId, planCode);
        return JsonSerializer.Serialize(new { success, employeeId, planCode });
    }
}
```

Attribute usage guidance:

- `[AgentSkillResource]` can decorate a property or method.
  - Use a method when content must be computed at read time, or when it needs an `IServiceProvider`.
- `[AgentSkillScript]` applies to methods only.
  - Scripts can also accept `IServiceProvider` for dependency-injected services.

Compose file-based and class-based skills together with `AgentSkillsProviderBuilder`:

```csharp
// dotnet add package Contoso.Skills.HrEnrollment
var skillsProvider = new AgentSkillsProviderBuilder()
    .UseFileSkill(Path.Combine(AppContext.BaseDirectory, "skills"))
    // file-based: onboarding guide
    .UseSkill(new BenefitsEnrollmentSkill())
    // class-based: benefits enrollment skill from NuGet package
    .UseFileScriptRunner(SubprocessScriptRunner.RunAsync)
    // runner for file-based scripts
    .Build();
```

The article notes that no custom routing logic is needed: skills are advertised in the agent’s prompt and the agent decides which skill to load based on the user’s request.

## Step 3: Bridging a gap with an inline code-defined skill

If a skill isn’t available yet as a package, define it inline with `AgentInlineSkill`:

```csharp
using System.Text.Json;
using Microsoft.Agents.AI;

var timeOffSkill = new AgentInlineSkill(
    name: "time-off-balance",
    description: "Calculate an employee's remaining vacation and sick days. Use when asked about available time off or leave balances.",
    instructions: """
    Use this skill when an employee asks how many vacation or sick days they have left.
    1. Ask for the employee ID if not already provided.
    2. Use the calculate-balance script to get the remaining balance.
    3. Present the result clearly, showing both used and remaining days.
    """)
    .AddScript("calculate-balance", (string employeeId, string leaveType) =>
    {
        // Temporary implementation – replace with the NuGet skill when available
        int totalDays = HrDatabase.GetAnnualAllowance(employeeId, leaveType);
        int daysUsed = HrDatabase.GetDaysUsed(employeeId, leaveType);
        int remaining = totalDays - daysUsed;
        return JsonSerializer.Serialize(new { employeeId, leaveType, totalDays, daysUsed, remaining });
    });
```

Add it alongside the others:

```csharp
var skillsProvider = new AgentSkillsProviderBuilder()
    .UseFileSkill(Path.Combine(AppContext.BaseDirectory, "skills"))
    // file-based: onboarding guide
    .UseSkill(new BenefitsEnrollmentSkill())
    // class-based: benefits enrollment skill from NuGet package
    .UseSkill(timeOffSkill)
    // code-defined: temporary bridge
    .UseFileScriptRunner(SubprocessScriptRunner.RunAsync)
    // runner for file-based scripts
    .Build();
```

Inline skills also support dynamic resources via `.AddResource()` factories:

```csharp
var hrPoliciesSkill = new AgentInlineSkill(
    name: "hr-policies",
    description: "Current HR policies on leave, remote work, and conduct.",
    instructions: "Read the policies resource and answer the employee's question.")
    .AddResource("policies", () =>
    {
        // Fetched live so the agent always sees the latest version
        return PolicyRepository.GetActivePolicies();
    });
```

The article calls out other good fits for inline skills:

- Skills generated at runtime (for business units/regions)
- When scripts/resources need to close over call-site state (instead of using DI)

## Step 4: Requiring approval before scripts run

When scripts have real side effects (query production infrastructure, write to HR systems), add a human approval checkpoint:

```csharp
var skillsProvider = new AgentSkillsProviderBuilder()
    .UseFileSkill(Path.Combine(AppContext.BaseDirectory, "skills"))
    // file-based: onboarding guide
    .UseSkill(new BenefitsEnrollmentSkill())
    // class-based: benefits enrollment skill from NuGet package
    .UseSkill(timeOffSkill)
    // code-defined: temporary time-off balance bridge
    .UseFileScriptRunner(SubprocessScriptRunner.RunAsync)
    // runner for file-based scripts
    .UseScriptApproval(true)
    .Build();
```

Behavior:

- When the agent wants to run a script, it returns an approval request instead of executing immediately.
- Your application collects an approve/reject decision and resumes the agent.

Reference: Tool approval documentation: https://learn.microsoft.com/en-us/agent-framework/agents/tools/tool-approval?pivots=programming-language-csharp

## Filtering from shared directories

If you have a shared skill library and only want a subset, use a filter predicate:

```csharp
var approvedSkills = new HashSet<string> { "onboarding-guide", "benefits-enrollment" };

var skillsProvider = new AgentSkillsProviderBuilder()
    .UseFileSkill(Path.Combine(AppContext.BaseDirectory, "all-skills"))
    .UseFilter(skill => approvedSkills.Contains(skill.Frontmatter.Name))
    .Build();
```

## Links

- Agent Skills documentation: https://learn.microsoft.com/en-us/agent-framework/agents/skills
- .NET samples: https://github.com/microsoft/agent-framework/tree/main/dotnet/samples/02-agents/AgentSkills
- GitHub Discussions: https://github.com/microsoft/agent-framework/discussions


[Read the entire article](https://devblogs.microsoft.com/agent-framework/agent-skills-in-net-three-ways-to-author-one-provider-to-run-them/)

