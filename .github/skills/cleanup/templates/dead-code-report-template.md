# Dead Code Detection Report

**Date**: {DATE}  
**Branch**: {BRANCH}

---

## Summary

| Category | Count | Confidence |
| -------- | ----- | ---------- |
| Unused Using Statements | {UNUSED_USINGS_COUNT} | High |
| Unused Private Members | {UNUSED_MEMBERS_COUNT} | Medium-High |
| Commented Code Blocks | {COMMENTED_CODE_COUNT} | Medium |
| Unused CSS Classes | {UNUSED_CSS_COUNT} | Low-Medium |

**Total Items**: {TOTAL_DEAD_CODE_COUNT}

---

## 🟢 High Confidence (Safe to Remove)

### Unused Using Statements ({UNUSED_USINGS_COUNT})

{UNUSED_USINGS_DETAILS}

---

## 🟡 Medium Confidence (Review Before Removing)

### Unused Private Members ({UNUSED_MEMBERS_COUNT})

{UNUSED_MEMBERS_DETAILS}

### Commented Code Blocks ({COMMENTED_CODE_COUNT})

{COMMENTED_CODE_DETAILS}

---

## 🟠 Low Confidence (Manual Review Required)

### Unused CSS Classes ({UNUSED_CSS_COUNT})

{UNUSED_CSS_DETAILS}

---

## Common False Positives to Watch For

- ❌ xUnit test methods (discovered by reflection)
- ❌ Blazor component parameters (set via Razor)
- ❌ Minimal API handlers (registered in `Program.cs`)
- ❌ Private fields only used in `Dispose()` methods
- ❌ Reflection-based code (check for attributes)

---

## What To Do Next

**Tell me what to remove**:

1. Review the findings above
2. Verify suspicious items if needed (ask "Show me [item]")
3. Respond with one of:
   - **"Remove all high confidence items"** - Remove unused usings
   - **"Remove unused usings only"** - Safe cleanup
   - **"Show me [specific item] before removing"** - Manual review
   - **"Skip dead code removal"** - All items are false positives

**Example responses**:

- "Remove all high confidence items" - I'll remove unused usings
- "Show me CalculateTotal method in PricingService.cs" - I'll show you the code
- "Remove unused usings only" - I'll skip members/comments/CSS

---

## Quick Response Template

```markdown
### My Decisions

High Confidence Items:
- {Action}: {Reason}

Medium Confidence Items:
- {Action}: {Reason}

Low Confidence Items:
- {Action}: {Reason}
```
