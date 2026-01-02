# Feature Specification: Component Testing with bUnit

**Priority**: 🔴 CRITICAL - Phase 2 (Implement alongside unit/integration tests)  
**Created**: 2026-01-02  
**Status**: Placeholder  
**Implementation Order**: Should be spec #5-6, not #23 (alongside 002-unit-testing and 003-integration-testing)

## Overview

Define comprehensive component testing strategy for Blazor components using bUnit, covering UI rendering, user interactions, state management, and integration with services.

## Why This Is Critical Early

- **Blazor components are the UI** - Can't validate they work without component tests
- **Unit tests don't cover rendering** - Need bUnit to test Blazor markup
- **Catch UI bugs early** - Before manual testing in browser
- **TDD for components** - Write tests as components are developed
- **Regression prevention** - Ensure changes don't break existing UI

## Scope

**Test Coverage**:
- Component rendering (markup output, CSS classes, attributes)
- User interactions (clicks, inputs, form submissions)
- Component parameters and cascading values
- Event callbacks and EventCallback<T> handling
- JavaScript interop mocking
- Service injection in components
- Component lifecycle (OnInitialized, OnParametersSet, etc.)
- Error boundaries and error handling

**Components to Test**:
- Navigation components (Header, Footer, SectionNav, CollectionNav)
- Filter components (DateFilter, TagFilter, SearchBox, FilterPanel)
- Content display (ItemCard, ItemList, ItemDetail)
- Infinite scroll component
- Video embed component
- Roundup components
- Loading states and error states

**Testing Patterns**:
- Arrange: Set up component parameters and mocked services
- Act: Trigger user interactions or lifecycle events
- Assert: Verify rendered markup and component state

## Implementation Note

⚠️ **This spec is numbered 023 but should be implemented in Phase 2**, alongside 002-unit-testing and 003-integration-testing. All testing strategies should be defined BEFORE starting major implementation.

## Status

📝 **Placeholder** - Needs bUnit setup instructions, test examples for each component type, mocking strategies, and CI integration.

## References

- [bUnit Documentation](https://bunit.dev)
- [Testing Blazor Components Best Practices](https://learn.microsoft.com/aspnet/core/blazor/test)
