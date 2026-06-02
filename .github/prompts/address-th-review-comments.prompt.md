---
name: address-th-review-comments
description: "Reviews all open review comment threads, CodeQL code scanning alerts, and GitHub Advanced Security alerts on the current branch's pull request, analyses each one, applies code fixes where needed, replies to each thread explaining what was done or why it was ignored, resolves each thread after replying, then commits and pushes directly."
model: Claude Sonnet 4.6
---

# Address PR Review Comments, CodeQL Alerts, and Advanced Security Alerts

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

## Step 5 — Fetch all open issues (review threads + security alerts)

### 5a — Fetch unresolved review threads

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

Parse the file. Filter to threads where `isResolved` is `false`. Count them and store as `[THREAD_COUNT]`.

### 5b — Fetch open code scanning (CodeQL) alerts

Fetch all open code scanning alerts for the branch:

```pwsh
gh api "repos/[REPO]/code-scanning/alerts?ref=refs/heads/[BRANCHNAME]&state=open&per_page=100" | Out-File -FilePath ".tmp/pr-codeql-alerts.json" -Encoding utf8
```

Parse the file. Each alert has: `number`, `rule.id`, `rule.description`, `rule.severity`, `most_recent_instance.location` (`path`, `start_line`, `end_line`), `most_recent_instance.message.text`, and `html_url`.

If the command fails (e.g., code scanning is not enabled), record 0 alerts and continue.

Count the open alerts and store as `[CODEQL_COUNT]`.

### 5c — Fetch open secret scanning alerts

Fetch all open secret scanning alerts:

```pwsh
gh api "repos/[REPO]/secret-scanning/alerts?state=open&per_page=100" | Out-File -FilePath ".tmp/pr-secret-alerts.json" -Encoding utf8
```

Parse the file. Each alert has: `number`, `secret_type_display_name`, `resolution`, `html_url`, and `locations_url`.

If the command fails (e.g., secret scanning is not enabled), record 0 alerts and continue.

Count the open alerts and store as `[SECRET_COUNT]`.

### 5d — Summarise what was found

If ALL three counts are zero, inform the user:

> No open review threads, CodeQL alerts, or secret scanning alerts found on PR #[PR_NUMBER]. Nothing to do.

Then skip directly to Step 10.

Otherwise report:

> Found [THREAD_COUNT] unresolved review thread(s), [CODEQL_COUNT] open CodeQL alert(s), and [SECRET_COUNT] open secret scanning alert(s) to address.

**CHECKPOINT**: "✅ Step 5 completed. [THREAD_COUNT] thread(s), [CODEQL_COUNT] CodeQL alert(s), [SECRET_COUNT] secret alert(s). Moving to Step 6."

---

## Step 6 — Analyse and address each open review thread

If `[THREAD_COUNT]` is 0, skip this step entirely.

**🚨 CRITICAL**: Steps 6d (reply) and 6e (resolve) are **MANDATORY** for **every single thread** — including threads where you made a code fix. You are not done with a thread until you have BOTH posted a reply AND resolved it on GitHub. Never skip 6d or 6e. Never batch them for later.

Work through each unresolved thread one at a time, in order. **Complete all six sub-steps before moving to the next thread.**

For **each thread**, do the following:

### 6a — Read and understand the thread

Read ALL comments in the thread carefully. Understand:

- **What file and line** the comment is on (use `path` and `line`)
- **What is being requested** (code change, explanation, style fix, etc.)
- **The diff hunk** for context on what the original code looked like

Read the relevant section of the file being discussed to understand the current code state.

### 6b — Decide: fix or explain

Determine one of two outcomes:

**NEEDS A FIX** — The comment points to a genuine issue: a bug, a missing guard, a logic error, a violated convention, a security concern, or any other substantive code problem that should be corrected.

**NO FIX NEEDED** — The comment is a style preference, is outdated (the issue no longer exists), is already addressed elsewhere, is overly opinionated without a clear correctness argument, or is something the current code intentionally does differently for good reason.

### 6c — If NEEDS A FIX: apply the fix

Make the minimal correct change to address the comment. Follow the conventions in the relevant `AGENTS.md` files for the file being changed. Run `get_errors` after editing to ensure no new errors were introduced.

Do **not** change anything beyond what the comment requests.

### 6d — **MANDATORY**: Reply to the thread

**🚨 Do this now, before moving to 6e. Do not defer.** Get the `databaseId` of the **first** comment in this thread (this is the root comment to reply to) and post a reply immediately.

**If you fixed it:**

```pwsh
gh api repos/[REPO]/pulls/[PR_NUMBER]/comments/[COMMENT_DATABASE_ID]/replies -X POST -f body="Fixed: [one or two sentences describing exactly what was changed and why, referencing the specific file and line if helpful]"
```

**If no fix was needed:**

```pwsh
gh api repos/[REPO]/pulls/[PR_NUMBER]/comments/[COMMENT_DATABASE_ID]/replies -X POST -f body="No change needed: [one or two sentences explaining why this comment does not require a fix — be specific and respectful]"
```

Verify the command exits 0. If it fails, stop and report the error.

### 6e — **MANDATORY**: Resolve the thread

**🚨 Do this now, immediately after 6d. Do not defer.** Use the thread `id` (the GraphQL node ID, not the `databaseId`) to mark the thread resolved on GitHub:

```pwsh
gh api graphql -f query='
mutation {
  resolveReviewThread(input: { threadId: "[THREAD_NODE_ID]" }) {
    thread { isResolved }
  }
}'
```

Verify the response shows `"isResolved": true`. If not, stop and report the error.

> **Note on dismiss vs resolve**: GitHub does not have a separate "dismiss" API for inline review threads — resolving IS the correct action. The reply you posted in 6d already makes the rationale visible. Resolving removes it from the open-discussions list so the PR stays clean.

### 6f — Checkpoint for this thread

Confirm both actions completed, then state:

"✅ Thread [N/TOTAL] — replied and resolved. [FIXED/NO_FIX]: [Brief one-line summary of action taken]."

---

Repeat steps 6a–6f for every unresolved thread before moving on.

**CHECKPOINT**: "✅ Step 6 completed. All [THREAD_COUNT] threads replied to and resolved on GitHub. Moving to Step 7."

---

## Step 7 — Analyse and address each open CodeQL alert

If `[CODEQL_COUNT]` is 0, skip this step entirely.

Work through each open CodeQL alert one at a time. **Complete all sub-steps before moving to the next alert.**

### 7a — Read and understand the alert

For each alert, read:

- **Rule**: `rule.id` and `rule.description` — what vulnerability or code quality issue was detected
- **Severity**: `rule.severity` (e.g., `error`, `warning`, `note`)
- **Location**: `most_recent_instance.location.path` and `start_line` — the exact file and line
- **Message**: `most_recent_instance.message.text` — the specific diagnostic message

Read the relevant file around the flagged line to understand the current code in context.

### 7b — Decide: fix or dismiss

**NEEDS A FIX** — The alert points to a genuine security issue, vulnerability, or code defect that should be corrected (e.g., SQL injection risk, unvalidated input, exposed secret, use of a deprecated insecure API).

**DISMISS** — The alert is a false positive, the code path is unreachable, the risk is mitigated elsewhere, or the flagged pattern is an intentional and safe design choice.

**🚨 CRITICAL**: For `error`-severity alerts, default to fixing unless there is a clear, well-reasoned case for dismissal.

### 7c — If NEEDS A FIX: apply the fix

Make the minimal correct change to resolve the CodeQL finding. Follow the conventions in the relevant `AGENTS.md` files. Run `get_errors` after editing to ensure no new errors were introduced.

### 7d — **MANDATORY**: Dismiss or note the alert on GitHub

**If you fixed the code**: The alert will auto-close when the fix is pushed. No API call needed here — just make a note that this alert was fixed in code.

**If dismissing (false positive / won't fix)**: Dismiss the alert via the API:

```pwsh
gh api repos/[REPO]/code-scanning/alerts/[ALERT_NUMBER] -X PATCH -f state="dismissed" -f dismissed_reason="false positive" -f dismissed_comment="[One or two sentences explaining why this is a false positive or won't be fixed, referencing the specific code path or mitigation]"
```

Valid `dismissed_reason` values: `"false positive"`, `"won't fix"`, `"used in tests"`.

Verify the command exits 0. If it fails, stop and report the error.

### 7e — Checkpoint for this alert

State:

"✅ CodeQL alert [N/TOTAL] (#[ALERT_NUMBER] — [rule.id]) — [FIXED in code / DISMISSED]. [Brief one-line summary of action taken]."

---

Repeat steps 7a–7e for every open CodeQL alert before moving on.

**CHECKPOINT**: "✅ Step 7 completed. All [CODEQL_COUNT] CodeQL alerts addressed. Moving to Step 8."

---

## Step 8 — Analyse and address each open secret scanning alert

If `[SECRET_COUNT]` is 0, skip this step entirely.

Work through each open secret scanning alert one at a time.

### 8a — Read and understand the alert

For each alert:

- Note the `secret_type_display_name` (e.g., "GitHub Personal Access Token")
- Fetch the alert locations to find where in the codebase the secret appears:

  ```pwsh
  gh api [LOCATIONS_URL]
  ```

- Identify the file(s) and line(s) involved.

### 8b — Decide: rotate or dismiss

**ROTATE / REMEDIATE** — The secret is a real credential or token that should not be in source code. The correct fix is to:

1. Remove the secret from the file (replace with an environment variable reference, a secrets manager reference, or a placeholder)
2. Immediately rotate/revoke the actual secret in the relevant system (GitHub, Azure Key Vault, etc.) — **inform the user** that rotation is needed since you cannot do this on their behalf

**DISMISS** — The value is a test fixture, a clearly fake/placeholder value, or is already rotated and the alert is stale.

**🚨 CRITICAL**: Never leave a real secret in the codebase. If in doubt, treat it as real and remediate.

### 8c — If ROTATE / REMEDIATE: remove the secret from code

Replace the hardcoded secret with an appropriate reference (e.g., `Environment.GetEnvironmentVariable("SECRET_NAME")`, a config binding, or a Key Vault reference). Follow existing patterns in the codebase. Run `get_errors` after editing.

**Then inform the user**:

> ⚠️ Secret `[SECRET_TYPE]` was found at `[FILE]:[LINE]`. The hardcoded value has been removed from the code. **You must manually rotate this secret** in [the relevant system] to ensure the exposed value is no longer valid.

### 8d — **MANDATORY**: Resolve or dismiss the alert on GitHub

**If you removed the secret from code**: The alert will auto-close when the fix is pushed. No API call needed.

**If dismissing**: Dismiss via the API:

```pwsh
gh api repos/[REPO]/secret-scanning/alerts/[ALERT_NUMBER] -X PATCH -f resolution="used_in_tests" -f resolution_comment="[Brief explanation]"
```

Valid `resolution` values: `"false_positive"`, `"wont_fix"`, `"revoked"`, `"used_in_tests"`.

### 8e — Checkpoint for this alert

State:

"✅ Secret alert [N/TOTAL] (#[ALERT_NUMBER] — [secret_type_display_name]) — [REMEDIATED in code / DISMISSED]. [Brief one-line summary]."

---

Repeat steps 8a–8e for every open secret scanning alert before moving on.

**CHECKPOINT**: "✅ Step 8 completed. All [SECRET_COUNT] secret scanning alerts addressed. Moving to Step 9."

---

## Step 9 — Verify no new errors

Run:

```pwsh
Run -Clean
```

If there are build or test failures caused by changes made in Steps 6, 7, or 8, fix them before proceeding.

**CHECKPOINT**: "✅ Step 9 completed. No errors or failures. Moving to Step 10."

---

## Step 10 — Summarise changes for the user

Print a concise summary with three sections:

**Review threads:**

| # | File | Line | Action | Summary |
|---|------|------|--------|------|
| 1 | ... | ... | Fixed / No fix | ... |

**CodeQL alerts:**

| # | Alert # | Rule | Severity | Action | Summary |
|---|---------|------|----------|--------|------|
| 1 | ... | ... | ... | Fixed / Dismissed | ... |

**Secret scanning alerts:**

| # | Alert # | Type | Action | Summary |
|---|---------|------|--------|------|
| 1 | ... | ... | Remediated / Dismissed | ... |

If a category had 0 items, omit its table and note "None found."

**CHECKPOINT**: "✅ Step 10 completed. Summary provided. Moving to Step 11."

---

## Step 11 — Commit and push directly

If **no code changes** were made (all issues received "no fix" / "dismiss" responses), skip this step — no commit is necessary.

Otherwise, stage all changed files:

```pwsh
git add -A
```

Write a short, direct commit message summarising the fixes (no ticket numbers, no PR references). Use imperative mood. Include all relevant issue types. Examples:

- `"Address PR review comments: [brief summary]"`
- `"Fix CodeQL alerts: [brief summary]"`
- `"Address review comments and fix CodeQL alerts: [brief summary]"`

```pwsh
git commit -m "[COMMIT_MESSAGE]"
```

Pull with rebase to incorporate any upstream changes, then push:

```pwsh
git pull --rebase origin [BRANCHNAME]
git push origin [BRANCHNAME]
```

If the push is rejected for any reason, stop and ask the user.

**CHECKPOINT**: "✅ Step 11 completed. Changes committed and pushed. Workflow complete."
