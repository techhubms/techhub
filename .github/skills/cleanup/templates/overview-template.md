# Code Quality Overview

**Date**: {DATE}  
**Branch**: {BRANCH}  
**Commit**: {COMMIT_SHA}

## Build Status

**Result**: ✅ Success / ❌ Failed  
**Errors**: {ERROR_COUNT}  
**Warnings**: {WARNING_COUNT}

---

## Issues Found

### High Priority (Fix Immediately)

**Total**: {HIGH_PRIORITY_COUNT} issues

#### Category: {CATEGORY_NAME} ({RULE_ID})

**Count**: {COUNT}  
**Severity**: Error / Warning  
**Recommendation**: Fix / Suppress / Refactor

**Description**: {BRIEF_DESCRIPTION}

**Affected Files**:

- [{FILE_PATH}]({FILE_PATH}#L{LINE}) - {SHORT_DESCRIPTION}
- [{FILE_PATH}]({FILE_PATH}#L{LINE}) - {SHORT_DESCRIPTION}

**Suggested Action**:
{DETAILED_EXPLANATION_OF_WHAT_TO_DO}

**Rationale**:
{WHY_THIS_MATTERS}

---

### Medium Priority (Should Fix)

**Total**: {MEDIUM_PRIORITY_COUNT} issues

#### Category: {CATEGORY_NAME} ({RULE_ID})

**Count**: {COUNT}  
**Severity**: Warning  
**Recommendation**: Fix / Suppress

**Description**: {BRIEF_DESCRIPTION}

**Affected Files**:

- [{FILE_PATH}]({FILE_PATH}#L{LINE}) - {SHORT_DESCRIPTION}

**Suggested Action**:
{DETAILED_EXPLANATION}

**Rationale**:
{WHY_THIS_MATTERS}

---

### Low Priority (Optional)

**Total**: {LOW_PRIORITY_COUNT} issues

#### Category: {CATEGORY_NAME} ({RULE_ID})

**Count**: {COUNT}  
**Severity**: Info / Warning  
**Recommendation**: Suppress / Fix if time permits

**Description**: {BRIEF_DESCRIPTION}

**Affected Files**:

- [{FILE_PATH}]({FILE_PATH}#L{LINE})

**Suggested Action**:
Suppress via .editorconfig or fix if straightforward

**Rationale**:
{OPTIONAL_EXPLANATION}

---

## Summary by Category

| Category | Rule ID | Count | Priority | Recommendation |
|----------|---------|-------|----------|----------------|
| {CATEGORY} | {RULE_ID} | {COUNT} | High / Medium / Low | Fix / Suppress |
| {CATEGORY} | {RULE_ID} | {COUNT} | High / Medium / Low | Fix / Suppress |
| {CATEGORY} | {RULE_ID} | {COUNT} | High / Medium / Low | Fix / Suppress |

**Total Issues**: {TOTAL_ISSUE_COUNT}  
**Recommended to Fix**: {FIX_COUNT}  
**Recommended to Suppress**: {SUPPRESS_COUNT}

---

## Recommendations

### Immediate Actions (High Priority)

1. {ACTION_DESCRIPTION}
2. {ACTION_DESCRIPTION}

### Next Steps (Medium Priority)

1. {ACTION_DESCRIPTION}
2. {ACTION_DESCRIPTION}

### Optional Improvements (Low Priority)

1. {ACTION_DESCRIPTION}
2. {ACTION_DESCRIPTION}

---

## Suppression Strategy

For issues recommended to suppress, add to [.editorconfig](/.editorconfig):

```ini
# {CATEGORY_NAME} - {REASON}
dotnet_diagnostic.{RULE_ID}.severity = none  # {JUSTIFICATION}
```

---

## Next Steps

1. Review this overview and decide on fix vs suppress strategy
2. For high-priority issues: Fix immediately before proceeding
3. For medium-priority issues: Fix or suppress based on project needs
4. For low-priority issues: Suppress to reduce noise

**Estimated Time**:

- High priority fixes: ~{ESTIMATE} minutes
- Medium priority fixes: ~{ESTIMATE} minutes
- Suppressions: ~{ESTIMATE} minutes
