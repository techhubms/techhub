---
external_url: https://github.blog/ai-and-ml/github-copilot/how-githubs-agentic-security-principles-make-our-ai-agents-as-secure-as-possible/
title: How GitHub’s Agentic Security Principles Make Copilot AI Agents Secure
author: Rahul Zhade
feed_name: The GitHub Blog
date: 2025-11-25 16:00:00 +00:00
tags:
- Action Attribution
- Agentic AI
- AI & ML
- AI Agents
- AI Risk Mitigation
- Copilot Coding Agent
- Data Exfiltration
- Firewall
- GitHub
- Human in The Loop
- Permissions
- Prompt Injection
- Security Principles
section_names:
- ai
- github-copilot
- security
---
Rahul Zhade details the agentic security principles behind GitHub Copilot’s AI agents, including threat models and practical measures to keep automated coding tools safe and user-attributable.<!--excerpt_end-->

# How GitHub’s Agentic Security Principles Make Copilot AI Agents Secure

GitHub has developed a set of security principles for its agentic AI products, particularly the Copilot coding agent, designed to maximize usability and security for developers. Rahul Zhade highlights the key risks associated with agentic features and shares actionable controls and guidelines for anyone building AI-driven automation tools.

## Threat Model and Security Concerns

When building AI agents capable of taking actions ("agentic"), GitHub observed that increased agent autonomy brings higher risk, such as:

1. **Data exfiltration**: Agents with internet access could inadvertently leak sensitive information, such as repository content or tokens, to unauthorized destinations.
2. **Impersonation and action attribution**: Misattributing actions can obscure who authorized or triggered a change, complicating traceability and accountability for incidents.
3. **Prompt injection**: Agents prompted from multiple sources (issues, files, comments) are vulnerable to maliciously hidden directives that can trick maintainers or expose systems to unwanted behaviors.

## Security Controls for Copilot Agentic Products

GitHub's guidelines are aimed at addressing these threats and are applicable to other AI automation agents:

### 1. Ensuring All Context is Visible

- Block usage of invisible Unicode or hidden HTML tags in prompts.
- Display all input sources (files, issues) to users, ensuring maintainers see exactly what the agent processes.

### 2. Firewalling the Agent

- Restrict agent network access with firewall rules, configurable by users.
- Block unsolicited external connections; only whitelisted Microsoft Copilot Platform (MCP) interactions are permitted automatically.
- For experiences like Copilot Chat, agent-generated output is sandboxed—no code executes automatically without explicit user action.

### 3. Limiting Access to Sensitive Information

- Provide agents only with the minimal data needed.
- CI secrets and files outside the repository are not passed to agents.
- Sensitive tokens are revoked as soon as the agent session ends.

### 4. Preventing Irreversible State Changes

- No direct commits to default branches; agents can only create pull requests subject to human review.
- CI pipelines are not triggered by agent-created PRs unless manually approved.
- In chat experiences, the agent requests approval before making tool calls.

### 5. Clear Attribution of Actions

- Every agent action is attributed both to the initiating user and the agent itself.
- Pull requests co-attributed to user and Copilot, indicating AI involvement.

### 6. Gathering Context Only from Authorized Users

- Only users with write permissions can assign the Copilot coding agent.
- Public repo agents only read issue comments from users with write access, preventing prompt injection from outsiders.

## Applying These Principles

The security controls described for Copilot's agentic features serve as a template for building secure AI agents elsewhere. By following these guidelines—including attributing actions, limiting context and capabilities, enforcing firewalls, and ensuring human oversight—developers can reduce the risk of automation gone awry.

For more, consult the [public documentation for Copilot coding agent](https://docs.github.com/en/copilot/concepts/agents/coding-agent/about-coding-agent) and explore [GitHub Copilot](https://github.com/features/copilot) to see these practices in action.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/ai-and-ml/github-copilot/how-githubs-agentic-security-principles-make-our-ai-agents-as-secure-as-possible/)
