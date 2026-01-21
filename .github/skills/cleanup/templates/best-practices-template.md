# Best Practices Review Report

**Date**: {DATE}  
**Branch**: {BRANCH}

---

## Summary

| Category | Count | Severity |
| -------- | ----- | -------- |
| 🔴 Critical Issues | {CRITICAL_COUNT} | Bugs, security, runtime errors |
| 🟡 Important Issues | {IMPORTANT_COUNT} | Performance, maintainability |
| 🟢 Minor Issues | {MINOR_COUNT} | Readability, style improvements |

**Total Issues**: {TOTAL_ISSUES}

---

## 🔴 Critical Issues ({CRITICAL_COUNT})

These issues can cause bugs, security vulnerabilities, or runtime errors. Fix immediately.

### Null Safety Issues

{NULL_SAFETY_ISSUES}

### Async/Await Issues

{ASYNC_AWAIT_ISSUES}

### Exception Handling Issues

{EXCEPTION_HANDLING_ISSUES}

### Disposal Issues

{DISPOSAL_ISSUES}

---

## 🟡 Important Issues ({IMPORTANT_COUNT})

These issues affect performance, maintainability, or code quality.

### Collection and LINQ Anti-Patterns

{LINQ_ISSUES}

### Performance Issues

{PERFORMANCE_ISSUES}

### Dependency Injection Issues

{DI_ISSUES}

### Magic Values and Configuration

{MAGIC_VALUES_ISSUES}

---

## 🟢 Minor Issues ({MINOR_COUNT})

These issues improve readability and maintainability but don't affect functionality.

### Code Readability

{READABILITY_ISSUES}

### String Handling

{STRING_ISSUES}

---

## Pattern Analysis

### Common Anti-Patterns Found

{COMMON_ANTIPATTERNS}

### Files with Most Issues

{FILES_WITH_MOST_ISSUES}

---

## What To Do Next

**Tell me which issues to fix**:

1. Review the issues above
2. For each category, respond with:
   - **"Fix {PATTERN}"** - I'll fix all occurrences
   - **"Show {FILE}"** - I'll show code for manual review
   - **"Fix all critical"** - I'll fix all 🔴 issues
   - **"Skip best practices review"** - All code is clean

**Example responses**:

- "Fix all critical" - I'll fix null safety and async issues
- "Fix .Count() on collections" - I'll change to .Count property
- "Show ContentRepository.cs" - I'll show file for manual review
- "Fix async void methods" - I'll change to async Task

---

## Quick Response Template

```markdown
### My Decisions

Critical Issues:
- {Action}: {Reason}

Important Issues:
- {Action}: {Reason}

Minor Issues:
- {Action}: {Reason}
```
