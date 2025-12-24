---
layout: "post"
title: "How to Create GitHub Issue Hierarchies via the API (REST & GraphQL Approaches)"
description: "Jesse Houwing explores newly added parent-child hierarchy support in GitHub Issues and demonstrates how to automate sub-issue creation using API calls. The guide covers pitfalls with the GitHub CLI, correct use of REST and GraphQL endpoints, and practical PowerShell scripting techniques."
author: "Jesse Houwing"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://jessehouwing.net/create-github-issue-hierarchy-using-the-api/"
viewing_mode: "external"
feed_name: "Jesse Houwing's Blog"
feed_url: "https://jessehouwing.net/rss/"
date: 2025-04-16 15:50:33 +00:00
permalink: "/posts/2025-04-16-How-to-Create-GitHub-Issue-Hierarchies-via-the-API-REST-and-GraphQL-Approaches.html"
categories: ["DevOps"]
tags: ["Automation", "CLI", "DevOps", "GitHub", "GitHub API", "GitHub Issues", "GitHub Projects", "GraphQL", "Issue Hierarchies", "Posts", "PowerShell", "Projects", "REST API"]
tags_normalized: ["automation", "cli", "devops", "github", "github api", "github issues", "github projects", "graphql", "issue hierarchies", "posts", "powershell", "projects", "rest api"]
---

In this technical walkthrough, Jesse Houwing explains how to automate the new parent-child hierarchy feature in GitHub Issues using REST and GraphQL APIs, along with CLI scripting tips.<!--excerpt_end-->

## Creating a GitHub Issue Hierarchy Using the API

**Author:** Jesse Houwing

GitHub recently introduced parent-child hierarchies within GitHub Issues, allowing for better issue organization and tracking. However, direct support for this tree structure hasn’t yet been implemented in the GitHub CLI, which can make automation challenging. This guide explores current limitations and provides practical workarounds using both REST and GraphQL APIs, along with PowerShell scripting examples.

---

### Initial Approach: REST API Issues

Attempting to create sub-issue relationships directly with the REST API, using the CLI, results in a 404 error:

```powershell
> '{"sub_issue_id": 12 }' | gh api https://api.github.com/repos/xebia/temp/issues/165/sub_issues -X post --input -
{ "message": "The provided sub-issue does not exist", "documentation_url": "https://docs.github.com/rest/issues/sub-issues#add-sub-issue", "status": "404" }
gh: The provided sub-issue does not exist (HTTP 404)
```

**Problem:** The error indicates the referenced sub-issue ID is not valid or not found, despite being based on documentation.

---

### Alternative: Using GraphQL API

The GitHub documentation and feature announcement suggest using the [GraphQL API](https://github.blog/changelog/2024-03-21-github-issues-adds-support-for-issue-types-and-parent-child-hierarchies/) for sub-issue management. **GraphQL implementation requires the internal issue ID, not the issue number.**

#### Steps

1. **Retrieve the Internal Issue IDs**
   - Use the GitHub CLI to query each issue’s internal ID:

     ```powershell
     $parent = gh issue view $parent --json id --jq ".id"
     $child = gh issue view $child --json id --jq ".id"
     ```

2. **Enable the Preview Header**
   - Pass the `GraphQL-Features: sub_issues` header to the API call.

3. **PowerShell Function**

    Example function to link parent and child issues:

    ```powershell
    function create-hierarchy {
      param(
        [string]$parent,  # e.g., https://github.com/{org}/{repo}/issues/{id}
        [string]$child
      )
      $ErrorActionPreference = "Stop"
      $parent = gh issue view $parent --json id --jq ".id"
      $child = gh issue view $child --json id --jq ".id"
      ---
layout: "post"
title: "How to Create GitHub Issue Hierarchies via the API (REST & GraphQL Approaches)"
description: "Jesse Houwing explores newly added parent-child hierarchy support in GitHub Issues and demonstrates how to automate sub-issue creation using API calls. The guide covers pitfalls with the GitHub CLI, correct use of REST and GraphQL endpoints, and practical PowerShell scripting techniques."
author: "Jesse Houwing"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://jessehouwing.net/create-github-issue-hierarchy-using-the-api/"
viewing_mode: "external"
feed_name: "Jesse Houwing's Blog"
feed_url: https://jessehouwing.net/rss/
date: 2025-04-16 15:50:33 +00:00
permalink: "2025-04-16-How-to-Create-GitHub-Issue-Hierarchies-via-the-API-REST-and-GraphQL-Approaches.html"
categories: ["DevOps"]
tags: ["Automation", "CLI", "DevOps", "GitHub", "GitHub API", "GitHub Issues", "GitHub Projects", "GraphQL", "Issue Hierarchies", "Posts", "PowerShell", "Projects", "REST API"]
tags_normalized: [["automation", "cli", "devops", "github", "github api", "github issues", "github projects", "graphql", "issue hierarchies", "posts", "powershell", "projects", "rest api"]]
---

In this technical walkthrough, Jesse Houwing explains how to automate the new parent-child hierarchy feature in GitHub Issues using REST and GraphQL APIs, along with CLI scripting tips.<!--excerpt_end-->

{{CONTENT}}

This post appeared first on "Jesse Houwing's Blog". [Read the entire article here](https://jessehouwing.net/create-github-issue-hierarchy-using-the-api/)
 = gh api graphql -H "GraphQL-Features: sub_issues" -f "query=mutation addSubIssue { addSubIssue(input: { issueId: \"$parent\", subIssueId: \"$child\" }) { issue { title } subIssue { title } } }"
    }
    ```

1. **Usage Example**

    ```powershell
    create-hierarchy \
      -parent https://github.com/jessehouwing/sample/1 \
      -child https://github.com/jessehouwing/sample/2
    ```

---

### Hierarchy Visualization

Once sub-issues are linked, the GitHub UI displays a tree structure:

![Parent/child issues screen](https://jessehouwing.net/content/images/2025/04/image.png)

You can further nest issues for multi-level hierarchies:

![Grandchild issue added](https://jessehouwing.net/content/images/2025/04/image-1.png)

In GitHub Projects, you can visualize progress by enabling **Parent** and **Sub issues progress** fields:

![GitHub Project with Parent/Sub-issue fields](https://jessehouwing.net/content/images/2025/04/image-2.png)

---

### Update: Correct Usage of Issue Identifiers

*It turns out* that `gh issue view --json id` actually returns the issue's `node_id` (used by GraphQL), **not** the numeric internal `id` expected by the REST API.

#### To use the REST API

1. **Get the Numeric Issue ID**

   ```powershell
   gh api https://api.github.com/repos/jessehouwing/sample/issues/1 --jq .id
   # Returns, e.g., 3000028010
   ```

2. **Add the Sub-Issue**

   ```powershell
   gh api https://api.github.com/repos/jessehouwing/sample/issues/1/sub_issues -X post -F sub_issue_id=3000028010
   ```

**Key Points:**

- The REST API endpoint URL uses the issue's number.
- The `sub_issue_id` must be the internal numeric ID (not the node_id string).

---

## Summary

- GitHub now supports parent-child hierarchies in Issues.
- The CLI currently lacks direct support for configuring issue trees.
- Both GraphQL and REST APIs can be used to create hierarchies, but require different identifiers (`node_id` for GraphQL, numeric `id` for REST).
- PowerShell scripting enables efficient automation of the hierarchy setup process.
- Once linked, parent/child relationships and progress are visible in GitHub UI and Projects.

---

## Useful References

- [GitHub Issues: Sub-issues Documentation](https://docs.github.com/rest/issues/sub-issues#add-sub-issue)
- [Introduction to Issue Hierarchies – GitHub Blog](https://github.blog/changelog/2024-03-21-github-issues-adds-support-for-issue-types-and-parent-child-hierarchies/)

This post appeared first on "Jesse Houwing's Blog". [Read the entire article here](https://jessehouwing.net/create-github-issue-hierarchy-using-the-api/)
