---
name: address-pr-reviews
description: "Reviews all open review comment threads on the current branch's pull request, analyses each one, applies code fixes where needed, replies to each thread explaining what was done or why it was ignored, then commits and pushes via the pushall workflow."
model: Claude Sonnet 4.6
---

# Address PR Review Comments

**🚨 CRITICAL**: Read this entire prompt from start to finish before executing any step.

**🚨 CRITICAL**: Execute every step in order. Do NOT skip, combine, or reorder steps.

**🚨 CRITICAL**: After every step write: "✅ Step [X] completed. Moving to Step [Y]." If a step fails, stop and ask the user.

**🚨 CRITICAL**: Exclusively use the `gh` CLI for all GitHub operations. Never use GitHub MCP tools.

**🚨 CRITICAL**: All terminal commands must be run in PowerShell (`pwsh`).

---

## Step 1 — Determine target branch

Run:

```pwsh
git branch --show-current
```

**If the result is `main` or `master`:**

Ask the user:

> You are on the main branch. Please provide either:
>
> - A **branch name** to check out, or
> - A **PR number** to look up the branch from

Wait for the user's answer.

- If the user gives a **PR number**: run the following to find the branch name and store it as `[BRANCHNAME]`:

  ```pwsh
  gh pr view [PR_NUMBER] --json headRefName -q '.headRefName'
  ```

- If the user gives a **branch name**: use it directly as `[BRANCHNAME]`.

**If the result is any other branch:**

Use it directly as `[BRANCHNAME]`.

**CHECKPOINT**: "✅ Step 1 completed. Target branch: [BRANCHNAME]. Moving to Step 2."

---

## Step 2 — Switch to branch and pull latest changes

If not already on `[BRANCHNAME]`, check it out:

```pwsh
git checkout [BRANCHNAME]
```

Then pull the latest remote changes with rebase:

```pwsh
git pull --rebase origin [BRANCHNAME]
```

If the pull fails due to conflicts, stop and ask the user how to proceed.

**CHECKPOINT**: "✅ Step 2 completed. On branch [BRANCHNAME] with latest changes. Moving to Step 3."

---

## Step 3 — Find the open pull request

```pwsh
gh pr list --head [BRANCHNAME] --state open --json number,title,url
```

If the output is empty (`[]`), inform the user:

> No open pull request found for branch `[BRANCHNAME]`. Nothing to do.

Then stop.

Otherwise, store the PR number as `[PR_NUMBER]` and the URL as `[PR_URL]`.

**CHECKPOINT**: "✅ Step 3 completed. PR #[PR_NUMBER]: [PR_URL]. Moving to Step 4."

---

## Step 4 — Get repository identity

```pwsh
gh repo view --json nameWithOwner -q '.nameWithOwner'
```

Store the result as `[REPO]` (format: `owner/repo`).

**CHECKPOINT**: "✅ Step 4 completed. Repo: [REPO]. Moving to Step 5."

---

## Step 5 — Fetch all open (unresolved) review threads

Run the following GraphQL query to retrieve all unresolved review threads and their comments. Replace `[OWNER]`, `[REPONAME]`, and `[PR_NUMBER]` with the actual values (split `[REPO]` on `/`):

```pwsh
gh api graphql -f query='
{
  repository(owner: "[OWNER]", name: "[REPONAME]") {
    pullRequest(number: [PR_NUMBER]) {
      reviewThreads(first: 100) {
        nodes {
          id
          isResolved
          isOutdated
          line
          path
          comments(first: 20) {
            nodes {
              databaseId
              body
              path
              line
              diffHunk
              author { login }
              createdAt
            }
          }
        }
      }
    }
  }
}
'
```

Save the full output to `.tmp/pr-review-threads.json`:

```pwsh
gh api graphql -f query='
{
  repository(owner: "[OWNER]", name: "[REPONAME]") {
    pullRequest(number: [PR_NUMBER]) {
      reviewThreads(first: 100) {
        nodes {
          id
          isResolved
          isOutdated
          line
          path
          comments(first: 20) {
            nodes {
              databaseId
              body
              path
              line
              diffHunk
              author { login }
              createdAt
            }
          }
        }
      }
    }
  }
}
' | Out-File -FilePath ".tmp/pr-review-threads.json" -Encoding utf8
```

Parse the file. Filter to threads where `isResolved` is `false`. If there are no unresolved threads, inform the user:

> All review threads on PR #[PR_NUMBER] are already resolved. Nothing to do.

Then skip directly to Step 9.

Otherwise, count the unresolved threads and report:

> Found [N] unresolved review thread(s) to address.

**CHECKPOINT**: "✅ Step 5 completed. Found [N] unresolved thread(s). Moving to Step 6."

---

## Step 6 — Analyse and address each open thread

Work through each unresolved thread one at a time, in order.

For **each thread**, do the following:

### 6a — Read and understand the thread

Read ALL comments in the thread carefully. Understand:

- **What file and line** the comment is on (use `path` and `line`)
- **What is being requested** (code change, explanation, style fix, etc.)
- **The diff hunk** for context on what the original code looked like

Read the relevant section of the file being discussed to understand the current code state:

```pwsh
# Read the file to understand current code
```

### 6b — Decide: fix or explain

Determine one of two outcomes:

**NEEDS A FIX** — The comment points to a genuine issue: a bug, a missing guard, a logic error, a violated convention, a security concern, or any other substantive code problem that should be corrected.

**NO FIX NEEDED** — The comment is a style preference, is outdated (the issue no longer exists), is already addressed elsewhere, is overly opinionated without a clear correctness argument, or is something the current code intentionally does differently for good reason.

### 6c — If NEEDS A FIX: apply the fix

Make the minimal correct change to address the comment. Follow the conventions in the relevant `AGENTS.md` files for the file being changed. Run `get_errors` after editing to ensure no new errors were introduced.

Do **not** change anything beyond what the comment requests.

### 6d — Reply to the thread

Get the `databaseId` of the **first** comment in this thread (this is the root comment to reply to).

Post a reply:

**If you fixed it:**

```pwsh
gh api repos/[REPO]/pulls/[PR_NUMBER]/comments/[COMMENT_DATABASE_ID]/replies -X POST -f body="Fixed: [one or two sentences describing exactly what was changed and why, referencing the specific file and line if helpful]"
```

**If no fix was needed:**

```pwsh
gh api repos/[REPO]/pulls/[PR_NUMBER]/comments/[COMMENT_DATABASE_ID]/replies -X POST -f body="No change needed: [one or two sentences explaining why this comment does not require a fix — be specific and respectful]"
```

### 6e — Checkpoint for this thread

State: "✅ Thread [N/TOTAL] addressed ([FIXED/NO_FIX]). [Brief one-line summary of action taken]."

---

Repeat steps 6a–6e for every unresolved thread before moving on.

**CHECKPOINT**: "✅ Step 6 completed. All [N] threads addressed. Moving to Step 7."

---

## Step 7 — Verify no new errors

Run:

```pwsh
Run -SkipE2E
```

If there are build or test failures caused by changes made in Step 6, fix them before proceeding.

**CHECKPOINT**: "✅ Step 7 completed. No errors or failures. Moving to Step 8."

---

## Step 8 — Summarise changes for the user

Print a concise table of every thread and what was done:

| # | File | Line | Action | Summary |
|---|------|------|--------|---------|
| 1 | ... | ... | Fixed / No fix | ... |

**CHECKPOINT**: "✅ Step 8 completed. Summary provided. Moving to Step 9."

---

## Step 9 — Commit and push via the pushall workflow

If any code changes were made in Step 6, load and follow the **pushall** skill:

> Read the file `.github/skills/pushall/SKILL.md` and follow the Push All Workflow from Step 1.

The pushall workflow will handle staging, committing, rebasing, pushing, and updating the pull request.

If **no code changes** were made (all threads received "no fix needed" replies), skip the pushall workflow — no commit is necessary.

**CHECKPOINT**: "✅ Step 9 completed. Workflow complete."
