# Code Quality Overview

**Date**: {DATE}  
**Branch**: {BRANCH}  
**Commit**: {COMMIT_SHA}

## Build Status

**Result**: {BUILD_RESULT}  
**Errors**: {ERROR_COUNT}  
**Warnings**: {WARNING_COUNT}

---

## 🔴 High Priority Issues (Fix Immediately)

**Total**: {HIGH_PRIORITY_COUNT}

These issues should be fixed before proceeding. They typically indicate bugs, security concerns, or critical quality issues.

{HIGH_PRIORITY_DETAILS}

---

## 🟡 Medium Priority Issues (Should Fix)

**Total**: {MEDIUM_PRIORITY_COUNT}

These issues should be addressed but can be suppressed if they are false positives or intentional design choices.

{MEDIUM_PRIORITY_DETAILS}

---

## 🟢 Low Priority Issues (Optional)

**Total**: {LOW_PRIORITY_COUNT}

These are stylistic or informational. Suppress via `.editorconfig` to reduce noise or fix if straightforward.

{LOW_PRIORITY_DETAILS}

---

## Summary

| Priority | Count | Action Required |
| -------- | ----- | --------------- |
| 🔴 High | {HIGH_PRIORITY_COUNT} | **Fix immediately** - Do not proceed until resolved |
| 🟡 Medium | {MEDIUM_PRIORITY_COUNT} | **Fix or suppress** - Review and decide |
| 🟢 Low | {LOW_PRIORITY_COUNT} | **Suppress or ignore** - Optional improvements |

**Total Issues**: {TOTAL_ISSUE_COUNT}

---

## What To Do Next

### ✅ If All Issues Are Low Priority

You can proceed safely. Consider suppressing these warnings in `.editorconfig`:

```ini
# Suppress low-priority warnings
{SUPPRESSION_EXAMPLES}
```

### ⚠️ If Medium/High Priority Issues Exist

**Tell me which issues to fix**:

1. Review the issue categories above
2. For each category, respond with one of:
   - **"Fix {RULE_ID}"** - I'll fix all occurrences
   - **"Suppress {RULE_ID}"** - I'll add `.editorconfig` suppression
   - **"Show {RULE_ID}"** - I'll show you the code for manual review

**Example responses**:

- "Fix CA1062" - I'll add null checks
- "Suppress IDE1006" - I'll suppress naming style warnings
- "Show RZ2012" - I'll show you the affected Blazor components

---

## Rule ID Reference

**Common Rule Prefixes**:

- **CA####** - Code Analysis (code quality, performance, security)
- **IDE####** - IDE suggestions (style, formatting, conventions)
- **RZ####** - Razor/Blazor specific issues
- **CS####** - C# compiler warnings

**Priority Mapping**:

- **High**: RZ\*, CA1001, CA1062, CA1304, CA1305, CA1307 (critical bugs/security)
- **Medium**: Most CA\* rules (code quality, maintainability)
- **Low**: IDE\*, CS15\* (style, documentation)

---

## Quick Response Guide

**Copy and modify this template for your response**:

```markdown
### My Decisions

High Priority:
- Fix {RULE_ID}: {brief reason}
- Suppress {RULE_ID}: {brief reason}

Medium Priority:
- Fix {RULE_ID}: {brief reason}
- Suppress {RULE_ID}: {brief reason}

Low Priority:
- Suppress all: {brief reason}
```
