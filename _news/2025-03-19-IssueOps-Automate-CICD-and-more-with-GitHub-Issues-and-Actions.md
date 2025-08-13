---
layout: "post"
title: "IssueOps: Automate CI/CD (and more!) with GitHub Issues and Actions"
description: "Nick Alteen explores IssueOps—turning GitHub Issues into command centers for automation. The article explains how to automate CI/CD, approvals, and more using GitHub Issues, GitHub Actions, and pull requests, providing detailed examples and workflow patterns, including a membership approval process."
author: "Nick Alteen"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/engineering/issueops-automate-ci-cd-and-more-with-github-issues-and-actions/"
viewing_mode: "external"
feed_name: "GitHub Engineering Blog"
feed_url: "https://github.blog/engineering/feed/"
date: 2025-03-19 16:00:24 +00:00
permalink: "/2025-03-19-IssueOps-Automate-CICD-and-more-with-GitHub-Issues-and-Actions.html"
categories: ["DevOps"]
tags: ["Approval Workflows", "Automation", "CI/CD", "DevOps", "Engineering", "Finite State Machine", "GitHub Actions", "GitHub Issues", "Issue Templates", "IssueOps", "News", "Pull Requests", "State Machines", "Team Management", "Workflow Automation"]
tags_normalized: ["approval workflows", "automation", "ci slash cd", "devops", "engineering", "finite state machine", "github actions", "github issues", "issue templates", "issueops", "news", "pull requests", "state machines", "team management", "workflow automation"]
---

In this comprehensive article, Nick Alteen introduces the concept of IssueOps, showing how GitHub Issues can be leveraged to automate CI/CD workflows, approval processes, and more using GitHub Actions and pull requests.<!--excerpt_end-->

# IssueOps: Automate CI/CD (and more!) with GitHub Issues and Actions

**Author: Nick Alteen**

Software development often involves managing a range of repetitive tasks—like handling issues, approvals, and triggering CI/CD workflows. IssueOps is a methodology that leverages GitHub Issues as a command center for such automation, reducing the need for manual intervention and enabling streamlined operations right within your repository.

## What is IssueOps?

IssueOps is the practice of using GitHub Issues, GitHub Actions, and pull requests as an interface for automating a variety of workflows. By utilizing issue comments, labels, and state changes, you can trigger everything from CI/CD pipelines and deployment actions to approvals and task assignments, all without leaving GitHub.

Like other \*Ops paradigms (e.g., ChatOps, ClickOps), IssueOps is a toolkit and strategy collection applied to GitHub Issues. Its tightly integrated relationship to pull requests allows an extensive set of possibilities, including workflow automation for managing approvals and deployments. IssueOps can be used not only for DevOps but for any workflow that can be interacted with through APIs. If it can be automated, IssueOps can probably handle it.

## Why Use IssueOps?

**Event-driven Automation:**

- Automate workflows from issues and pull requests.
- Trigger CI/CD pipelines, approvals, and updates directly based on GitHub actions or comments.

**Customizable:**

- Adapt workflows to specific team needs, from bug triage to deployment management.
- Customize based on events and user-supplied data.

**Transparency & Auditability:**

- Every action is logged on the issue timeline.
- Approvals and actions are all in one place for auditability.

## Designing IssueOps Workflows as State Machines

IssueOps workflows can be conceptualized as finite-state machines:

**Generic Workflow Example:**

1. User opens an issue with a request.
2. Issue is validated for required info.
3. Issue is submitted for processing.
4. Approval is requested from an authorized team/user.
5. Request is processed, issue is closed.

### Example: Team Membership Approval Workflow

- User requests team membership (issue is created).
- Request is validated.
- Approval is sought; admin reviews and approves/denies.
- If approved: User added to team.
- If denied: User is not added.
- User is notified of the outcome.

This workflow mirrors a finite-state machine (FSM):

- **States:** Opened, validated, submitted, approved, denied, closed.
- **Events:** Issue creation, comment added, label changes.
- **Transitions:** Movement from one state to another based on guards (conditions).
- **Actions:** Notifications, user additions, label updates.
- **Guards:** Check whether transitions (e.g., approval) should occur, such as verifying if an admin commented `.approve`.

[State Diagram Example]
![State diagram for a request approval process](https://github.blog/wp-content/uploads/2025/03/state-machine.png?resize=916%2C1588)

## Deep Dive: Workflow Implementation

### Step 1: Issue Form Template

Create a GitHub issue form to standardize user requests (e.g., for team membership):

```yaml
name: Team Membership Request
description: Submit a new membership request
title: New Team Membership Request
labels:
  - team-membership
body:
  - type: input
    id: team
    attributes:
      label: Team Name
      description: The team name you would like to join
      placeholder: my-team
      validations:
        required: true
```

This provides machine-readable JSON for further automation.

### Step 2: Validation

Use custom scripts and actions (e.g., [issue-ops/validator](https://github.com/issue-ops/validator)) to check the data:

```javascript
module.exports = async (field) => {
  const { Octokit } = require('@octokit/rest');
  const core = require('@actions/core');
  const github = new Octokit({ auth: core.getInput('github-token', { required: true }) });
  try {
    // Check if the team exists
    await github.rest.teams.getByName({ org: process.env.GITHUB_REPOSITORY_OWNER ?? '', team_slug: field });
    return 'success';
  } catch (error) {
    if (error.status === 404) {
      return `Team '${field}' does not exist`;
    } else {
      throw error;
    }
  }
};
```

### Step 3: Workflow Entrypoint

Set up a workflow that validates issues as they are opened, edited, or reopened. Example `Process Issue Open/Edit` uses validation and adds labels like `validated`.

### Step 4: Handling Comments — Triggers for Next Steps

- **Submit Request**: User comments `.submit`, triggering a workflow that re-validates and notifies admins for approval or denial.
- **Deny/Approval**: Admin comments `.deny` (closes issue and notifies user) or `.approve` (adds user to team, closes issue, and notifies user).
- Use guard clauses everywhere to ensure only valid transitions occur (e.g., only an admin can approve/deny).

### Sample Workflows

- **Approve Workflow**: Checks admin status, parses JSON, adds user to team, notifies, and closes issue.
- **Deny Workflow**: Notifies the user, closes the issue if denied by an admin.

## Extending IssueOps

IssueOps isn’t limited to membership management—the pattern can automate anything that suits an event-driven, state-machine process, including deployments, bug triage, or workflow auditing. Benefits include:

- Centralizing approvals and records within issues/pull requests.
- Enforcing processes and capturing an auditable record.
- Reducing manual errors and improving efficiency.

## Additional Resources

- [IssueOps documentation and open source examples](https://issue-ops.github.io/docs)

## Conclusion

By treating GitHub Issues as workflow entrypoints, IssueOps brings significant automation to teams and projects. Start small and tailor the pattern to your needs—workflow efficiency will increase as you iterate. Happy automating!

This post appeared first on "GitHub Engineering Blog". [Read the entire article here](https://github.blog/engineering/issueops-automate-ci-cd-and-more-with-github-issues-and-actions/)
