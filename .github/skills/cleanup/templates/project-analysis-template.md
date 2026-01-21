# Project Analysis Summary

**Date**: {DATE}  
**Branch**: {BRANCH}

---

## Implementation Inventory

### API Endpoints ({API_ENDPOINT_COUNT})

{API_ENDPOINTS_LIST}

### Blazor Components ({COMPONENT_COUNT})

{COMPONENTS_LIST}

### Services ({SERVICE_COUNT})

{SERVICES_LIST}

### Domain Models ({MODEL_COUNT})

{MODELS_LIST}

### Configuration

{CONFIGURATION_SUMMARY}

---

## Documentation Inventory

### API Specification

{API_DOCS_STATUS}

### Functional Docs ({FUNCTIONAL_DOCS_COUNT} files)

{FUNCTIONAL_DOCS_LIST}

### AGENTS.md Files ({AGENTS_FILES_COUNT} files)

{AGENTS_FILES_LIST}

---

## Test Coverage Inventory

### Unit Tests

- **Total Tests**: {UNIT_TESTS_COUNT}
- **Files**: {UNIT_TEST_FILES_COUNT}
- **Coverage**: {UNIT_TEST_COVERAGE}

{UNIT_TESTS_DETAILS}

### Integration Tests

- **Total Tests**: {INTEGRATION_TESTS_COUNT}
- **Files**: {INTEGRATION_TEST_FILES_COUNT}
- **Coverage**: {INTEGRATION_TEST_COVERAGE}

{INTEGRATION_TESTS_DETAILS}

### Component Tests

- **Total Tests**: {COMPONENT_TESTS_COUNT}
- **Files**: {COMPONENT_TEST_FILES_COUNT}
- **Coverage**: {COMPONENT_TEST_COVERAGE}

{COMPONENT_TESTS_DETAILS}

### E2E Tests

- **Total Tests**: {E2E_TESTS_COUNT}
- **Files**: {E2E_TEST_FILES_COUNT}
- **Coverage**: {E2E_TEST_COVERAGE}

{E2E_TESTS_DETAILS}

### PowerShell Tests

- **Total Tests**: {POWERSHELL_TESTS_COUNT}
- **Files**: {POWERSHELL_TEST_FILES_COUNT}

{POWERSHELL_TESTS_DETAILS}

---

## Gap Analysis

### 🔴 Critical Gaps

{CRITICAL_GAPS}

### 🟡 Important Gaps

{IMPORTANT_GAPS}

### 🟢 Nice to Have

{MINOR_GAPS}

---

## Summary

**Implementation**: {IMPLEMENTATION_SUMMARY}  
**Documentation**: {DOCUMENTATION_SUMMARY}  
**Testing**: {TESTING_SUMMARY}

**Overall Health**: {OVERALL_HEALTH_GRADE}

---

## What To Do Next

**Tell me how to proceed**:

1. Review the gap analysis above
2. Respond with one of:
   - **"Proceed with documentation review"** - Continue to Step 6
   - **"Fix critical gaps first"** - Address gaps before continuing
   - **"Add missing tests for [feature]"** - Fill specific test gaps
   - **"Document [feature]"** - Fill specific documentation gaps
   - **"Skip gap fixing"** - Note gaps but continue cleanup

**Example responses**:

- "Proceed with documentation review" - Continue with cleanup process
- "Add E2E tests for section filtering" - I'll create missing E2E tests
- "Document the RSS feed endpoints" - I'll add API documentation

---

## Quick Response Template

```markdown
### My Decisions

Critical Gaps:
- {Action}: {Reason}

Important Gaps:
- {Action}: {Reason}

Next Steps:
- {Decision}
```
