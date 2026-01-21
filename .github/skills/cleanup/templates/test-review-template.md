# Test Review Report

**Date**: {DATE}  
**Branch**: {BRANCH}

---

## Summary

| Category | Count | Severity |
| -------- | ----- | -------- |
| 🔴 Critical Issues | {CRITICAL_COUNT} | Broken tests, anti-patterns, incorrect positioning |
| 🟡 Important Issues | {IMPORTANT_COUNT} | Missing coverage, quality problems |
| 🟢 Minor Issues | {MINOR_COUNT} | Style, naming, documentation |

**Overall Test Quality Grade**: {TEST_QUALITY_GRADE}

---

## Test Statistics

| Test Type | Files | Tests | Status |
| --------- | ----- | ----- | ------ |
| Unit Tests | {UNIT_FILES} | {UNIT_TESTS} | {UNIT_STATUS} |
| Integration Tests | {INTEGRATION_FILES} | {INTEGRATION_TESTS} | {INTEGRATION_STATUS} |
| Component Tests | {COMPONENT_FILES} | {COMPONENT_TESTS} | {COMPONENT_STATUS} |
| E2E Tests | {E2E_FILES} | {E2E_TESTS} | {E2E_STATUS} |
| PowerShell Tests | {POWERSHELL_FILES} | {POWERSHELL_TESTS} | {POWERSHELL_STATUS} |

---

## 🔴 Critical Issues ({CRITICAL_COUNT})

These issues MUST be fixed - they indicate broken tests, serious anti-patterns, or fundamentally wrong test design.

{CRITICAL_ISSUES_DETAILS}

---

## 🟡 Important Issues ({IMPORTANT_COUNT})

These issues should be addressed - they affect test quality, maintainability, or coverage.

{IMPORTANT_ISSUES_DETAILS}

---

## 🟢 Minor Issues ({MINOR_COUNT})

These issues are optional improvements - they enhance readability and consistency.

{MINOR_ISSUES_DETAILS}

---

## Positioning Analysis

### Correctly Positioned Tests

{CORRECTLY_POSITIONED}

### Misplaced Tests

{MISPLACED_TESTS}

---

## Coverage Gaps

### Missing Test Coverage

{MISSING_COVERAGE}

### Edge Cases Not Tested

{MISSING_EDGE_CASES}

---

## Test Quality Grading

### Grade: {TEST_QUALITY_GRADE}

{GRADE_JUSTIFICATION}

**Grading Rubric**:

- **A (Excellent)**: 0 critical, 0-2 important issues
- **B (Good)**: 0 critical, 3-5 important issues
- **C (Acceptable)**: 0 critical, 6+ important OR 1-2 critical issues
- **D (Poor)**: 3-5 critical issues
- **F (Failing)**: 6+ critical issues OR tests don't run

---

## What To Do Next

**Tell me which issues to fix**:

1. Review the issues above
2. Respond with one of:
   - **"Fix all critical issues"** - I'll address broken/wrong tests
   - **"Move integration tests to use mocks"** - Fix API test positioning
   - **"Add missing edge case tests for [feature]"** - Fill coverage gaps
   - **"Fix AAA pattern violations"** - Add explicit Arrange/Act/Assert comments
   - **"Fix test names"** - Update names to match behavior
   - **"Skip test review"** - All tests are correct

**Example responses**:

- "Fix all critical issues" - I'll fix broken tests and anti-patterns
- "Move GetSections_ReturnsData test to use mocked repository" - Fix specific positioning issue
- "Add edge case tests for null inputs in ContentItemService" - Fill coverage gap
- "Fix test name: GetSections_ReturnsData should be GetSections_WithTagFilter_ReturnsFilteredData" - Rename misleading test

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
