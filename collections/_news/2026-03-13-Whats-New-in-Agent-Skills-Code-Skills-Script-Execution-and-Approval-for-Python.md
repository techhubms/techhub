---
layout: "post"
title: "What’s New in Agent Skills: Code Skills, Script Execution, and Approval for Python"
description: "This article explores recent enhancements to the Microsoft Agent Framework Python SDK, including code-defined skills, executable scripts, and script approval mechanisms for Agent Skills. It explains how these features enable flexible skill authoring, dynamic content sourcing, controlled script execution, and enterprise-grade safety measures such as human-in-the-loop approvals and auditability."
author: "Sergey Menshykh"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/agent-framework/whats-new-in-agent-skills-code-skills-script-execution-and-approval-for-python/"
viewing_mode: "external"
feed_name: "Microsoft Semantic Kernel Blog"
feed_url: "https://devblogs.microsoft.com/semantic-kernel/feed/"
date: 2026-03-13 11:04:33 +00:00
permalink: "/2026-03-13-Whats-New-in-Agent-Skills-Code-Skills-Script-Execution-and-Approval-for-Python.html"
categories: ["AI", "Coding"]
tags: ["Agent Framework", "Agent Skills", "Agents", "AI", "Automation", "Best Practices", "Code Defined Skills", "Coding", "DevOps Runbooks", "Dynamic Resources", "Enterprise AI", "Human in The Loop", "Microsoft Agent Framework", "News", "Python", "Script Execution", "SDK", "Skill Approval", "Skill Authoring"]
tags_normalized: ["agent framework", "agent skills", "agents", "ai", "automation", "best practices", "code defined skills", "coding", "devops runbooks", "dynamic resources", "enterprise ai", "human in the loop", "microsoft agent framework", "news", "python", "script execution", "sdk", "skill approval", "skill authoring"]
---

Sergey Menshykh details the latest updates to Microsoft Agent Framework for Python, introducing code-defined Agent Skills, flexible script execution, and approval flows for enterprise safety and control.<!--excerpt_end-->

# What’s New in Agent Skills: Code Skills, Script Execution, and Approval for Python

Sergey Menshykh introduces powerful updates to the Microsoft Agent Framework Python SDK, particularly focused on Agent Skills. These enhancements grant developers greater flexibility, finer-grained control, and improved safety when designing and deploying AI-driven agents.

## Highlights

- **Code-Defined Skills**: Define agent skills directly in Python code with dynamic resources, without relying solely on file system-based packages. Developers use code-based Skill objects and resource decorators for flexible, maintainable skill definitions.
- **Dynamic Resources**: Register functions as resources for skills, allowing retrieval of up-to-date information (e.g., environment variables, real-time team info) every time the resource is accessed.
- **Code-Defined and File-Based Scripts**: Use decorators to expose executable scripts directly in code for in-process execution, or provide file-based scripts handled via pluggable runners (such as sandboxed subprocess for Python files).
- **Script Approval Mechanism**: Enforce human-in-the-loop safety by requiring human approval to run agent scripts with sensitive or production-side effects, enhancing enterprise compliance and oversight.
- **Flexible Integration**: Combine file-based and code-defined skills in a single provider to suit varied team workflows.

## Practical Examples

### Creating Code-Defined Skills

Developers can construct skills within Python code for scenarios where skill content must be dynamic or co-located with application logic:

```python
from agent_framework import Skill, SkillResource, SkillsProvider
code_style_skill = Skill(
    name="code-style",
    description="Team coding conventions",
    content="Use this skill for coding style and best practices.",
    resources=[
        SkillResource(
            name="style-guide",
            content="""
                # Team Coding Style Guide
                - Use 4-space indentation (no tabs)
                - Max line length: 120 characters
                - Use type annotations on all public functions
            """
        )
    ]
)
skills_provider = SkillsProvider(skills=[code_style_skill])
```

### Dynamic Skill Resources

Register Python functions using the `@skill.resource` decorator to provide up-to-date, computed values from APIs, databases, or environment variables.

### Code-Defined Script Registration

Expose functions as scripts with `@skill.script`, allowing in-process execution:

```python
@unit_converter_skill.script(name="convert", description="Convert a value: result = value × factor")
def convert_units(value: float, factor: float) -> str:
    import json
    result = round(value * factor, 4)
    return json.dumps({"value": value, "factor": factor, "result": result})
```

### Script Runners for File-Based Scripts

Setup custom runners to handle file-based script execution with process isolation, input validation, and logging.

### Human Approval for Script Execution

Add enterprise safety by gating script execution behind approval with `require_script_approval=True`, pausing agents until human review is provided. This supports documented audit trails.

## Use Cases

- **Data Validation Pipelines**: Encapsulate data quality rules as validation scripts requiring data steward approval.
- **DevOps Runbooks**: Store operational runbooks as skills, with agent-driven execution requiring human sign-off for any changes.
- **Dynamic Organizational Knowledge**: Retrieve live information (e.g., HR policies, team roster) from internal systems using code-based resources for up-to-date agent answers.

## Security and Best Practices

- Always review third-party scripts for safety and intent.
- Use sandboxing and limit permissions for script execution environments.
- Enable script approval for any skill with potential side effects.
- Maintain full audit and logging for all skill and script operations.

## Getting Started

- **[Agent Skills Documentation on Microsoft Learn](https://learn.microsoft.com/en-us/agent-framework/agents/skills)**
- **[Python examples on GitHub](https://github.com/microsoft/agent-framework/tree/main/python/samples/02-agents/skills)**
- **[Discussion Boards](https://github.com/microsoft/agent-framework/discussions)**

These improvements make the Microsoft Agent Framework for Python a robust platform for creating safe, flexible, and enterprise-ready agent solutions.

This post appeared first on "Microsoft Semantic Kernel Blog". [Read the entire article here](https://devblogs.microsoft.com/agent-framework/whats-new-in-agent-skills-code-skills-script-execution-and-approval-for-python/)
