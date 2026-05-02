---
name: pushall
description: "Step-by-step workflow for safely and precisely executing code and GitHub operations according to strict instructions. Use this whenever asked to push code changes, create pull requests, or perform related operations to ensure a consistent and reliable process."
---

# Push All Workflow

**🚨 ABSOLUTE CRITICAL REQUIREMENT 1**: NEVER EVER use pattern recognition or "I know what this step should do" thinking. Each step has EXACT instructions - follow them literally, not what you think they should accomplish.

**🚨 ABSOLUTE CRITICAL REQUIREMENT 2**: Do NOT optimize for tokens, speed, or efficiency. This workflow is intentionally verbose and step-by-step for precision. Follow every sub-instruction within each step.

**🚨 ABSOLUTE CRITICAL REQUIREMENT 3**: Do NOT improvise, combine, reorder, parallelize or alter instructions in any way.

**🚨 ABSOLUTE CRITICAL REQUIREMENT 4**: First read the entire prompt from beginning to end so you understand all steps and can follow them properly.

**🚨 ABSOLUTE CRITICAL REQUIREMENT 5**: ALWAYS start with step 1, then execute the steps in this prompt in EXACTLY the order specified by the checkpoints. Do NOT improvise, optimize, skip, merge or deviate in ANY other way from the workflow.

**🚨 ABSOLUTE CRITICAL REQUIREMENT 6**: After each step, you must:

- Confirm the step completed successfully and you followed all instructions. If not, go back and do what you missed.
- State which step you're moving to next  
- Verify all required outputs exist before proceeding
- Verify you satisfied the conditions for the checkpoint and then write them out to the user: "✅ Step [X] completed successfully. Moving to Step [Y].".
- If you find yourself thinking "I know what this step wants me to accomplish" - STOP. Read the step again and follow the literal instructions.
- Never substitute your own interpretation for the written instructions, even if you think your approach is better or more efficient.

**🚨 ABSOLUTE CRITICAL REQUIREMENT 7**: Code blocks are EXACT COMMANDS to execute, not examples or suggestions. Execute them character-for-character as written, including all parameters and flags. Remember escaping in PowerShell happens with a backtick ` instead of a backslash \

**🚨 ABSOLUTE CRITICAL REQUIREMENT 8**: If something fails, goes wrong, something unexpected happens or you need to make a choice and the current workflow steps do not provide a clear answer or instruction, ALWAYS ask the user what to do instead of making assumptions or guessing.

**🚨 ABSOLUTE CRITICAL REQUIREMENT 9**: Exclusively use the `gh` CLI for GitHub related actions, such as listing, creating, and updating pull requests, or listing and requesting code reviews. **Never** use GitHub MCP tools

**🚨 ABSOLUTE CRITICAL REQUIREMENT 10**: Throughout this workflow, maintain these variables:

- `[BRANCHNAME]`: Current branch name (from step 2 analysis, updated in step 7 if branch operations performed)
- Update this section when variables change

**🚨 ABSOLUTE CRITICAL REQUIREMENT 11**: **ALWAYS trust git CLI output over any VS Code metadata, context attachments, or workspace information.** VS Code may provide stale or incorrect branch/repository information in its context. The `git` CLI is the single source of truth for all branch names, status, and repository state throughout this entire workflow. If there is ever a conflict between what VS Code shows and what a git command returns, the git command wins — always.

**🚨 CRITICAL PROMPT SCOPE**: All instructions, restrictions, and requirements in this prompt file ONLY apply when this specific prompt is being actively executed via the `/pushall` command or equivalent prompt invocation. These rules do NOT apply when editing, reviewing, or working with this file outside of prompt execution context. When working with this file in any other capacity (editing, debugging, documentation, etc.), treat it as a normal markdown file and ignore all workflow-specific instructions.

## Step-by-Step Git Commit, Rebase, Conflict Resolution, and Push Workflow

1. **MANDATORY AND CRITICAL, DO NOT SKIP**: This file contains 11 absolute critical requirements at the top. Make an optimized list for yourself without losing ANY instruction or the intent behind it and tell me you did this, without showing me the list. Use this checklist internally to validate you did everything correct. These are your 11 commandments.

2. **Check current branch:**

    First, refresh VS Code's git context by running the `git.refresh` VS Code command via the `run_vscode_command` tool with `commandId: git.refresh`. This ensures VS Code has the latest git state.

    Then run this command to get the current branch name:

    ```pwsh
    git branch --show-current
    ```

    **CRITICAL**: Set variable: [BRANCHNAME] = current branch name

    **CHECKPOINT**: "✅ Step 2 completed successfully. Current branch: [BRANCHNAME]. Moving to Step 3."

3. **Branch decision point:** Based on the current branch information gathered in step 2, determine next steps.

    **CHECKPOINT**: State either:
    - "✅ Step 3 completed successfully. On main branch. Moving to Step 4 to handle main branch protection steps."
    - "✅ Step 3 completed successfully. On feature branch [BRANCHNAME]. Moving to Step 7 to confirm the current branch."

4. **Handle main branch protection:** You are on the main branch and need to handle branch protection.

    First, examine the uncommitted changes using git to understand what was changed. Look inside the repository and analyze the actual changes - not only the filenames! When you have a clear understanding of the changes, create a proper branch name that describes the changes and their intent. Use the following guidelines to create the branch name:

    - Create a descriptive branch name (e.g., "feature/update-documentation", "fix/powershell-scripts", "enhancement/ui-improvements")
    - Use kebab-case format with appropriate prefix (feature/, fix/, enhancement/, etc.)
    - Keep the name concise but descriptive of the main changes

    **CRITICAL**: Set variable: [BRANCHNAME] = your created branch name

    Then use the delay script to ask if the user wants to move changes to a new branch:

    ```pwsh
    ./.github/skills/pushall/pushall-delay.ps1 -Warning "You are on the main branch. If you do nothing, a branch will be created for you with the name:" -Message "[BRANCHNAME]" -Delay 10
    ```

    **CHECKPOINT**: Based on exit code, state either:
    - **Exit code 0 (user did not abort):** "✅ Step 4 completed successfully. User accepted branch creation for [BRANCHNAME]. Moving to Step 5."
    - **Exit code 1 (user aborted):** "✅ Step 4 completed successfully. User aborted branch creation. Moving to Step 6."

5. **Create new branch from main:** Move changes from main to a new branch.

    First, soft reset any existing commits on main to preserve all changes as uncommitted:

    ```pwsh
    git reset --soft HEAD~$(git rev-list --count HEAD ^origin/main)
    ```

    **CRITICAL**: If the new branch already exists, delete it first! We should ALWAYS have a clean branch to work from.

    Then create and switch to a new branch:

    ```pwsh
    git checkout -b [BRANCHNAME]
    ```

    **CHECKPOINT**: "✅ Step 5 completed successfully. Created and switched to branch. Moving to Step 7."

6. **Handle branch creation abort:** Handle the case when user aborts branch creation from main.

    Ask the user how they want to handle the branch situation and help them create or switch to the appropriate branch.

    **CHECKPOINT**: "✅ Step 6 completed successfully. Issue resolved. Moving to Step 7."

7. **Confirm branch:** You are not on the main branch. Confirm the user wants to continue on the current branch.

    ```pwsh
    ./.github/skills/pushall/pushall-delay.ps1 -Warning "If you do nothing, your changes will be pushed on branch:" -Message "[BRANCHNAME]" -Delay 10
    ```

    **CHECKPOINT**: Based on exit code, state either:
    - **Exit code 0 (user did not abort):** "✅ Step 7 completed successfully. User confirmed working on branch [BRANCHNAME]. Moving to Step 8."
    - **Exit code 1 (user aborted):** Help the user fix the issue (switching branches, etc.), then state: "✅ Step 7 completed successfully. Issue resolved. Moving to Step 8."

8. **Comprehensive PREPARATION FOR COMMIT:**

    **CRITICAL**: This is only preparation. Do NOT commit until you are told.
    **CRITICAL**: This step is DIFFERENT than what you will do later in step 17. Do NOT confuse them.
    **CRITICAL**: This step ALWAYS executes regardless of which path you took to reach it. Step 2 only checked the current branch - we have NOT properly analyzed ALL changes yet. This is where the comprehensive analysis happens.

    **Call the get-git-changes script to perform the first comprehensive analysis of all workspace changes:**

    ```pwsh
    ./.github/skills/pushall/get-git-changes.ps1
    ```

    This script captures comprehensive git changes analysis, including git status, individual diff files, and branch information, saving it to `.tmp/git-changes-analysis/` directory. The main analysis data is in `git-changes-analysis.json` and individual `.diff` files are created for each changed file.

    After the script completes, check if the file exists:

    ```pwsh
    if (-not (Test-Path ".tmp/git-changes-analysis/git-changes-analysis.json")) { throw "Analysis file was not created. Script may have failed." }
    ```

    **CRITICAL**: After successful verification, perform a really thorough analysis of ALL changes:**

    - Review the `summary` object to understand the scope of changes (totalFiles, new, modified, deleted, renamed)
    - Check the `files` object for individual file changes grouped by change type:
      - `files.new[]`: Array of new files - each object contains:
        - `file`: The file path
        - `diffPath`: Path to the diff file for this specific change (if available)
      - `files.modified[]`: Array of modified files - each object contains:
        - `file`: The file path  
        - `diffPath`: Path to the diff file for this specific change
      - `files.deleted[]`: Array of deleted files - each object contains:
        - `file`: The file path
      - `files.renamed[]`: Array of renamed files - each object contains:
        - `file`: The file path
        - `diffPath`: Path to the diff file for this specific change (if available)
    - Use exact `diffPath` values from each file object - do NOT assume filename formats
    - **New files**: Examine `files.new[]` array - Read the new file contents directly, because there are no diffs available since they're new
    - **Modified files**: Examine `files.modified[]` array - Use the individual `.diff` files via the `diffPath` from each file object for detailed understanding of what changed
    - **Deleted files**: Examine `files.deleted[]` array - Diff files show what has been deleted.
    - **Renamed files**: Examine `files.renamed[]` array - Diff files show rename and any content changes. Follow the modified files instructions for these.  
    - Check `branch.current` for the current branch name
    - Check `branch.remote.exists` to know if the branch already exists in the remote server
    - Check `branch.remote.hasUpdates` to know if the remote branch has updates we need to pull in
    - Check `branch.main.hasUpdates` to know if remote main has updates we need to rebase against
    - If any of the properties are missing or appear invalid, let the user know and do NOT continue!

    **CHECKPOINT**: "✅ Step 8 completed successfully. Comprehensive git analysis complete. Current branch: [BRANCHNAME]. Moving to Step 9."

9. **Create commit MESSAGE:** Based on the comprehensive changes analysis from step 8, prepare a structured commit message:

    **Commit Message Structure:**
    - For the summary, focus on the PURPOSE and INTENT behind the changes, not just what was changed. The implementation can be more technical.
    - Use bullet points that explain WHY you're making these changes.

    ```text
    [Brief descriptive title - max 50 characters]

    Summary: [Brief explanation of what this commit does and why - max 50 words, prefer less]

    Implementation:
    • [Specific technical changes made to the codebase]
    • [Key files modified and how they were changed]
    • [Architecture or design patterns introduced or modified]
    ```

    **Example STRUCTURED commit message:**

    ```text
    Improve developer workflow automation reliability

    Summary: Enhanced git workflow automation with better change analysis and structured commit generation to reduce developer friction and improve process reliability.

    Implementation:
    • Enhanced pushall SKILL.md with comprehensive change analysis and branch protection
    • Improved get-git-changes.ps1 with unified diff generation and file categorization
    • Added structured commit message templates and PowerShell command standardization
    ```

    **CRITICAL**: After creating the structured commit message, write it directly to `.tmp/git-changes-analysis/commit-message.txt` file using this exact command format:

    ```pwsh
    @"
    [Your actual commit message content here]
    "@ | Out-File -FilePath ".tmp/git-changes-analysis/commit-message.txt" -Encoding utf8
    ```

    Replace `[Your actual commit message content here]` with the commit message you prepared.

    **CHECKPOINT**: "✅ Step 9 completed successfully. Commit message prepared and saved. Moving to Step 10."

10. **Get user confirmation:** Use the delay script to confirm the commit message you prepared:

    ```pwsh
    ./.github/skills/pushall/pushall-delay.ps1 -Warning "If you do nothing, your changes will be committed and pushed on branch '[BRANCHNAME]' with message:" -Message "" -MessageFile ".tmp/git-changes-analysis/commit-message.txt" -Delay 20
    ```

    **CHECKPOINT**: Based on exit code, state either:
    - **Exit code 0 (user did not abort):** "✅ Step 10 completed successfully. User confirmed commit message. Moving to Step 11."
    - **Exit code 1 (user aborted):** "❌ Step 10 aborted by user. Asking what's wrong and stopping workflow."

11. **Stage all changes:**

    ```pwsh
    git add .
    ```

    **CHECKPOINT**: "✅ Step 11 completed successfully. All changes staged. Moving to Step 12."

12. **Commit with the confirmed message:**

    ```pwsh
    git commit -F ".tmp/git-changes-analysis/commit-message.txt"
    ```

    **CHECKPOINT**: "✅ Step 12 completed successfully. Changes committed with prepared message. Moving to Step 13."

13. **Synchronize with remote branch:** After committing, ensure your branch is synchronized with the remote branch to get the latest changes.

    Read the `branch.remote.exists` and `branch.remote.hasUpdates` fields from the git-changes-analysis.json file in the `.tmp/git-changes-analysis/` directory.

    **If `branch.remote.exists` is false OR `branch.remote.hasUpdates` is false:**
    Skip the remote pull operation.

    **If `branch.remote.exists` is true and `branch.remote.hasUpdates` is true:**

    ```pwsh
    git pull --rebase
    ```

    **CRITICAL**: If the rebase stops for any reason, use the [Branch Rebase Instructions](#branch-rebase-instructions) section below. When you are done rebasing, you must continue with the main workflow steps, starting from the checkpoint in this step.

    **CHECKPOINT**: "✅ Step 13 completed successfully. Remote branch synchronization complete. Moving to Step 14."

14. **Synchronize with main branch:** Ensure your branch maintains a clean history by rebasing onto the remote main branch.

    Read the `branch.main.hasUpdates` field from the git-changes-analysis.json file in the `.tmp/git-changes-analysis/` directory.

    **If `branch.main.hasUpdates` is false:**
    Your branch is already up-to-date with main.

    **If `branch.main.hasUpdates` is true:**

    First, fetch the latest changes from the remote main branch:

    ```pwsh
    git fetch origin main
    ```

    Then rebase onto the remote main branch (not local main):

    ```pwsh
    git rebase origin/main
    ```

    **CRITICAL**: If the rebase stops for any reason, use the [Branch Rebase Instructions](#branch-rebase-instructions) section below. When you are done rebasing, you must continue with the main workflow steps, starting from the checkpoint in this step.

    **CHECKPOINT**: "✅ Step 14 completed successfully. Main branch synchronization complete. Moving to Step 15."

15. **Push your changes:**

    Use the `branch.remote.exists` field to determine the appropriate push command.

    **If `branch.remote.exists` is false (new branch):**

    ```pwsh
    git push --set-upstream origin [BRANCHNAME]
    ```

    **If `branch.remote.exists` is true (existing branch):**

    If you rebased in step 13 or step 14, the branch history has been rewritten. Use `--force-with-lease` to push safely:

    ```pwsh
    git push --force-with-lease
    ```

    If no rebase occurred, use a normal push:

    ```pwsh
    git push
    ```

    **CHECKPOINT**: "✅ Step 15 completed successfully. Changes pushed to remote branch [BRANCHNAME]. Moving to Step 16."

16. **Ask user about pull request creation/updating:**

    Use the delay script to ask the user if they want to proceed with pull request operations:

    ```pwsh
    ./.github/skills/pushall/pushall-delay.ps1 -Warning "If you do nothing, the system will start with PR operations." -Delay 30
    ```

    **CHECKPOINT**: Based on exit code, state either:
    - **Exit code 0 (user did not abort):** "✅ Step 16 completed successfully. User wants to proceed with PR operations. Moving to Step 17."
    - **Exit code 1 (user aborted):** "✅ Step 16 completed successfully. User aborted PR operations. Moving to Step 21."

17. **PREPARE for data analysis:**

    **CRITICAL**: This step is about data PREPARATION only. Do NOT analyze or create PR content yet.
    **CRITICAL**: This step is DIFFERENT than what you did earlier in steps 8 to 15, so do NOT confuse them and DO NOT skip any of the following steps.
    **CRITICAL**: Do NOT reuse anything from step 8 to 15.
    **CRITICAL**: Execute the following command EXACTLY AS IS including the `-CompareWithMain` flag:

    ```pwsh
    ./.github/skills/pushall/get-git-changes.ps1 -CompareWithMain
    ```

    **CHECKPOINT**: "✅ Step 17 completed successfully. Moving to Step 18."

18. **ANALYZE data from step 17 and PREPARE pull request TITLE and MESSAGE:**

    **CRITICAL**: Do NOT continue until you have done EVERYTHING in this step. This is crucial for creating a meaningful pull request.
    **CRITICAL**: This is analysis and preparation only. Do NOT create a pull request until you are told.
    **CRITICAL**: Using the data collected in step 17, analyze ALL changes mentioned in `.tmp/git-changes-analysis/git-changes-analysis.json` and make sure you have answers to ALL questions listed below.
    **CRITICAL**: If you need to look at other files in this repository to get a complete picture, do that as well!

    **Questions on WHAT was done (Functional Changes):**
    - What specific functionality was added, modified, or removed?
    - What user-facing or developer-facing capabilities changed?
    - What systems, components, or processes were affected?

    **Questions on WHY it was necessary (Problem & Context):**
    - What problem or need drove these changes?
    - What pain points were addressed?
    - How do these changes align with recent repository activity?
    - What value do they provide to the project and its users?

    **Questions on HOW functionality changed (Old vs New):**
    - What was the previous behavior or state?
    - What is the new behavior or state?
    - What workflows or experiences are different now?
    - What are the key technical improvements or architectural changes?

    **Questions on Change Type:**
    Depending on the primary change type, answer the correct question:
    - New Feature: What new capability was added?
    - Bug Fix: What issue was resolved and how?
    - Improvement: What was enhanced and what's better now?
    - Refactoring: What structure was improved and why?
    - Documentation: What knowledge was added or updated?
    - Configuration: What setup or deployment changes were made?

    **Questions on Impact:**
    - User Impact: How will end users experience these changes?
    - Developer Impact: How will developers work differently?
    - System Impact: What internal processes or behaviors changed?
    - Quality Impact: How do these changes improve code quality, performance, or maintainability?

    As the FINAL thing in step 18, do the following:

    **CRITICAL**: Synthesize your analysis into the following and store it INTERNALLY for later use:
    - A clear, descriptive PR title (focused on the primary functional change)
    - A PR description that tells the story at a high level: problem → solution → impact, do not make it very extensive. Then follow with the technical changes. You do not need to include the answers to all the questions listed above.

    **CHECKPOINT**: "✅ Step 18 completed successfully. Pull request PREPARATION complete. Moving to Step 19."

19. **CHECK for existing pull requests:**

    Read the `branch.remote.exists` field from `.tmp/git-changes-analysis/git-changes-analysis.json` to determine if there can be existing pull requests.

    **CRITICAL**: Do NOT create or update a PR yet, we will do that in the next step.

    **If `branch.remote.exists` is true (remote branch exists):**

    Check if there is already an open pull request for the current branch:

    ```pwsh
    gh pr list --head [BRANCHNAME] --state open --json number,title,url
    ```

    If the command returns a PR:
    - Note its number for step 20
    - Fetch the existing PR title and body so you can build on them in step 20:

      ```pwsh
      gh pr view [PR_NUMBER] --json title,body
      ```

    - Read and understand the existing PR title and body content. Store them INTERNALLY for use in step 20.

    **If `branch.remote.exists` is false (no remote branch):**

    - Do nothing, because there is no remote branch and there can be no pull request.

    **CHECKPOINT**: "✅ Step 19 completed successfully. Existing PR check complete. Moving to Step 20."

20. **UPDATE existing PR or CREATE new PR:**

    Based on the outcome of step 19, either update an existing PR or create a new one.

    **CRITICAL**: After this step you are NOT done yet, keep following the steps!

    **If there is an existing PR:**

    **CRITICAL**: Do NOT replace the existing PR description wholesale. Instead, MERGE the new analysis (from step 18) with the existing PR content (fetched in step 19) to produce an updated description. Follow these rules:

    - **Preserve** existing information that is still accurate and relevant
    - **Add** new information about the latest changes that is missing from the existing description
    - **Update** sections where the content has changed (e.g., revised behavior, updated file lists)
    - **Remove** information that is no longer accurate (e.g., references to files or behavior that no longer exist in the branch)
    - **Keep** the same structural format and section headings as the existing description when possible
    - For the **title**: keep the existing title if it still accurately describes the overall PR scope; only update it if the scope has meaningfully changed

    Write the merged PR description to `.tmp/git-changes-analysis/pr-description.txt`:

    ```pwsh
    @"
    [Your merged PR description here]
    "@ | Out-File -FilePath ".tmp/git-changes-analysis/pr-description.txt" -Encoding utf8
    ```

    Then update the pull request:

    ```pwsh
    gh pr edit [PR_NUMBER] --title "[PR_TITLE]" --body-file ".tmp/git-changes-analysis/pr-description.txt"
    ```

    - Do NOT display the updated PR information. This will happen later.

    **If there is no existing PR:**

    Use the exact PR title and description you prepared in step 18.

    Write the PR description to `.tmp/git-changes-analysis/pr-description.txt`:

    ```pwsh
    @"
    [Your actual PR description here]
    "@ | Out-File -FilePath ".tmp/git-changes-analysis/pr-description.txt" -Encoding utf8
    ```

    Create a pull request with the head branch set to your current branch and base branch to main:

    ```pwsh
    gh pr create --base main --head [BRANCHNAME] --title "[PR_TITLE]" --body-file ".tmp/git-changes-analysis/pr-description.txt"
    ```

    - Note the PR number from the output for subsequent steps.
    - Do NOT display the created PR information. This will happen later.

    **CHECKPOINT**: "✅ Step 20 completed successfully. PR created/updated with prepared title and description. Moving to Step 21."

21. **Workflow completion and final summary**

This step provides the final workflow summary and completion status.

**Execute the following actions:**

a. **Display completion header:**

   ```text
   ✅ **PUSHALL WORKFLOW COMPLETED SUCCESSFULLY** ✅
   ```

b. **Provide final workflow summary** with the following sections:

- **🎯 Final Workflow Summary**: Brief overview of what was accomplished
- **🎉 Workflow Execution**: Steps completed, execution mode, duration, final result

c. **Include relevant links** (if PR was created/updated):

- Link to the pull request with descriptive text

**Checkpoint:** ✅ Step 21 completed successfully. Workflow execution summary provided and pushall process is complete.

## Branch Rebase Instructions

These are reusable instructions for rebasing onto any target branch (main, remote branch, etc.) to keep branches synchronized and minimize conflicts.

**Pre-Rebase Setup:**

- Ensure all current changes are staged and committed before starting rebase
- Note the current branch name for reference
- Identify the target branch you're rebasing onto

**Execute Rebase:**
Use the appropriate rebase command based on your situation:

- For remote main branch: `git fetch origin main` then `git rebase origin/main`
- For remote branch updates: `git pull --rebase` (already in progress)
- For continuing interrupted rebase: `git rebase --continue`

**Handle Rebase Results:**

If the rebase completes successfully, the branch is now synchronized with the target branch. Continue with the next step in your workflow.
If the rebase stops for any reason, follow these 4 steps, labelled a, b, c, d:

a. **Check rebase status:** Determine why the rebase stopped:

   ```pwsh
   git status
   ```

b. **Handle different scenarios:**

   **Scenario 1: Merge conflicts:**

- Identify and investigate conflicted files (marked as "both modified" in git status)
- **CRITICAL** Inform the user about the conflicts, give them your suggestion on what to do and **ask them what you should do**.
- Wait for user confirmation that conflicts are resolved
- Stage resolved files and continue:

     ```pwsh
     git add .
     git rebase --continue
     ```

   **Scenario 2: Rebase paused but no issues:**

- If git status shows "rebase in progress" but no conflicts or changes, simply continue:

     ```pwsh
     git rebase --continue
     ```

- This happens when Git pauses for review or when commits apply cleanly but Git wants confirmation

   **Scenario 3: Empty commits:**

- If git specifically reports "No changes - did you forget to use 'git add'?" or "nothing to commit":

     ```pwsh
     git rebase --skip
     ```

- This happens when a commit becomes empty during rebase (e.g., changes were already applied)

   **Scenario 4: Other unexpected issues:**

- For any other problems, STOP and ask the user how to proceed
- Do NOT automatically skip commits or make assumptions
- Common options: continue, skip (only if user confirms), or abort

   **Scenario 5: If rebase needs to be aborted:**

- User can choose to abort the rebase:

     ```pwsh
     git rebase --abort
     ```

- This returns to the state before the rebase started
c. **Repeat until rebase completes:** Continue handling interruptions until git rebase finishes successfully
d. **Rebase completion:** The rebase is complete when git no longer reports conflicts and returns to the normal prompt

**Post-Rebase Validation:**

**CRITICAL**: After successful rebase completion, continue WHERE YOU LEFT OFF in the main workflow.
