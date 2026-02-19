# Query Logging

## Overview

The Tech Hub application includes optional database query logging functionality that can help with debugging and performance analysis. When enabled, it logs:

- SQL queries executed
- Parameter values (sanitized)
- Execution time in milliseconds
- Row counts returned
- **Automatic EXPLAIN plans** for queries exceeding 1 second

## Configuration

Query logging is controlled through [DatabaseOptions](../src/TechHub.Core/Configuration/DatabaseOptions.cs) in your `appsettings.json`:

```json
{
  "Database": {
    "Provider": "PostgreSQL",
    "ConnectionString": "Host=localhost;Port=5432;Database=techhub;Username=techhub;Password=localdev",
    "EnableQueryLogging": true
  }
}
```

### Settings

| Setting | Type | Default | Description |
|---------|------|---------|-------------|
| `EnableQueryLogging` | `bool` | `false` | Enable detailed query logging with SQL, parameters, and timing |

## Security Considerations

⚠️ **WARNING**: Query logging may expose sensitive data in logs:

- Parameter values are logged (though sanitized for fields containing "password", "secret", or "token")
- Long strings are truncated to 100 characters
- Use only in development or controlled debugging scenarios
- **Never enable in production** unless absolutely necessary and logs are secured

## Implementation

Query logging is implemented via Dapper extension methods in [DapperExtensions.cs](../src/TechHub.Infrastructure/Data/DapperExtensions.cs):

- `QueryWithLoggingAsync<T>()` - wraps `QueryAsync` with logging
- `QueryMultipleWithLoggingAsync()` - wraps `QueryMultipleAsync` with logging

These extensions:

1. Start a stopwatch before query execution
2. Execute the query
3. Log the SQL, parameters, execution time, and row count
4. Handle exceptions and log failures
5. **If query exceeds 1 second**: automatically run EXPLAIN and log the execution plan

## Slow Query Detection

Queries taking longer than 1 second (configurable via `DapperExtensions.SlowQueryThresholdMs`) are automatically flagged:

- Logged at **Warning** level instead of Information
- Prefixed with `SLOW QUERY:` for easy identification
- **EXPLAIN plan** is automatically executed and logged alongside the slow query

### EXPLAIN Output

The EXPLAIN feature detects the database provider automatically:

- **PostgreSQL**: Runs `EXPLAIN (ANALYZE, BUFFERS)` for detailed execution statistics

Example slow query log output:

```text
[Warning] SLOW QUERY: 1523ms, Rows=20
SQL: SELECT MAX(tag_display) AS Tag, COUNT(*) AS Count FROM content_tags_expanded...
Params: DynamicParameters(5 params)

[Warning] EXPLAIN plan for slow query:
  GroupAggregate  (cost=0.42..1234.56 rows=200 width=40) (actual time=1.2..1520.0 rows=20 loops=1)
    ->  Index Only Scan using idx_tags_collection_tagword on content_tags_expanded  (cost=0.42..1100.00 rows=5000 width=32)
         Heap Fetches: 0
         Buffers: shared hit=150
  Planning Time: 0.5 ms
  Execution Time: 1521.3 ms
```

## Output Format

Normal queries appear at Information level:

```text
[Information] Query executed: 45ms, Rows=12
SQL: SELECT slug, title, date_epoch...
Params: collection="blogs", take=20, skip=0
```

## Usage

Query logging is automatically used by [ContentRepository](../src/TechHub.Infrastructure/Repositories/ContentRepository.cs) when `EnableQueryLogging` is `true`. No code changes are required.

To enable logging temporarily:

1. Edit `appsettings.json` or `appsettings.Development.json`
2. Set `Database:EnableQueryLogging` to `true`
3. Restart the application
4. Observe logs for query details
5. Watch for `SLOW QUERY` warnings and their EXPLAIN plans

## Related

- [Database Configuration](database.md)
- [Testing Strategy](testing-strategy.md)
