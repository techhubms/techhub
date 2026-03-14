---
external_url: https://jessehouwing.net/create-github-issue-hierarchy-using-the-api/
tags:
- Automation
- Blogs
- DevOps
- Gh API
- GitHub
- GitHub CLI
- GitHub GraphQL API
- GitHub Issues
- GitHub Projects
- GitHub REST API
- GraphQL Features Header
- Issue Hierarchy
- Issue ID
- Node Id
- Parent Child Issues
- PowerShell
- Preview Features
- Sub Issues
feed_name: Jesse Houwing's Blog
section_names:
- devops
author: Jesse Houwing
date: 2025-04-16 15:50:33 +00:00
title: Create GitHub issue hierarchy using the API
primary_section: devops
---
Jesse Houwing shows how to create parent-child issue hierarchies (sub-issues) in GitHub by using the GitHub CLI with GraphQL and REST API calls, including the key detail that REST requires the issue’s internal numeric id (not the node_id).<!--excerpt_end-->

## Overview

GitHub Issues now supports **issue types** and **parent-child hierarchies** (sub-issues). At the time of writing, the **GitHub CLI (`gh`)** doesn’t provide a direct command to set up this hierarchy, but you can do it via the **GitHub REST API** or **GitHub GraphQL API**.

This write-up covers:

- Why a direct REST call may return **404** even when the endpoint is documented
- A working GraphQL approach (using issue **node IDs** and a feature header)
- A corrected REST approach (using the issue’s internal numeric **id**, not `node_id`)

## The REST API attempt (404)

A direct REST call to add a sub-issue can fail with a 404 like:

```pwsh
"{""sub_issue_id"": 12 }" | gh api https://api.github.com/repos/xebia/temp/issues/165/sub_issues -X post --input -
{ "message": "The provided sub-issue does not exist", "documentation_url": "https://docs.github.com/rest/issues/sub-issues#add-sub-issue", "status": "404" } gh: The provided sub-issue does not exist (HTTP 404)
```

## GraphQL approach: use issue IDs (with a feature header)

The GraphQL calls for sub-issues don’t use issue numbers; they use the issue’s **GraphQL ID**. You also need to pass a preview/feature header to enable `sub_issues`:

- Header: `GraphQL-Features: sub_issues`

### PowerShell function

```pwsh
function create-hierarchy {
  param(
    # https://github.com/{org}/{repo}/issues/{id}
    [string]$parent,
    # https://github.com/{org}/{repo}/issues/{id}
    [string]$child
  )

  $ErrorActionPreference = "Stop"

  $parent = gh issue view $parent --json id --jq ".id"
  $child = gh issue view $child --json id --jq ".id"

  $_ = gh api graphql -H "GraphQL-Features: sub_issues" -f "query=mutation addSubIssue { addSubIssue(input: { issueId: \"\"$parent\"\", subIssueId: \"\"$child\"\" }) { issue { title } subIssue { title } } }"
}
```

### Example usage

```pwsh
create-hierarchy `
  -parent https://github.com/jessehouwing/sample/1 `
  -child https://github.com/jessehouwing/sample/2
```

### Result in the UI

After running this, the parent issue shows the sub-issue relationship in GitHub Issues.

- ![GitHub UI showing an issue #1 titled "parent" with a sub-issue #2 called "child"](https://jessehouwing.net/content/images/2025/04/image.png)

Nested hierarchies (a “grandchild” issue) are also visible at each level.

- ![GitHub UI showing the same parent/child issues, now with an added #3 grandchild](https://jessehouwing.net/content/images/2025/04/image-1.png)

You can also visualize progress in **GitHub Projects** by adding the **Parent** and **Sub issues progress** fields.

- ![GitHub Project configured with Parent and Sub-issues status fields enabled.](https://jessehouwing.net/content/images/2025/04/image-2.png)

## Update: the correct REST API approach (use numeric `id`, not `node_id`)

Further investigation shows that:

- `gh issue view --json id` returns a value that corresponds to the issue’s GraphQL `node_id`
- The REST endpoint expects `sub_issue_id` to be the issue’s internal numeric `id`

Example REST issue payload fields:

```json
{ "url": "https://api.github.com/repos/jessehouwing/sample/issues/1", "id": 3000028010, "node_id": "I_kwDOOakzpM6yyU6H", "number": 1 ... }
```

So, for the REST call you must first fetch the issue’s numeric `id`:

```pwsh
gh api https://api.github.com/repos/jessehouwing/sample/issues/1 --jq .id
3000028010
```

Then add the sub-issue using that numeric ID:

```pwsh
gh api https://api.github.com/repos/jessehouwing/sample/issues/1/sub_issues -X post -F sub_issue_id=3000028010
```

### Key detail

- The REST URL uses the issue’s `number` (e.g., `/issues/1/...`)
- The `sub_issue_id` field must be the internal numeric `id` of the child issue

## References

- GitHub REST docs: https://docs.github.com/rest/issues/sub-issues#add-sub-issue


[Read the entire article](https://jessehouwing.net/create-github-issue-hierarchy-using-the-api/)

