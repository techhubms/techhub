# Core Unit Tests

> **RULE**: Follow [tests/AGENTS.md](../AGENTS.md) for shared testing rules and [Root AGENTS.md](../../AGENTS.md) for workflow.

Unit tests for the Core domain layer. Validates domain models, business logic, and pure functions in complete isolation.

## What to Test

- ✅ Domain model behavior (`GetUrlInSection`, URL generation, property calculations)
- ✅ Business rules and validation
- ✅ Edge cases (null, empty, boundary values)
- ✅ Extension methods (pure functions)

## What NOT to Test Here

- ❌ C# language features (record equality, property getters)
- ❌ Simple property assignments with no logic
- ❌ External dependencies (file I/O, HTTP — use mocks)

## Key Patterns

- Use `A.ContentItem`, `A.Section`, `A.Collection` builders from TestUtilities for test data
- Use `[Theory]` with `[InlineData]` for parameterized scenarios
- No I/O, no external dependencies — pure logic only
