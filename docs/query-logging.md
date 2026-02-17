# Query Logging

## Overview

The Tech Hub application includes optional database query logging functionality that can help with debugging and performance analysis. When enabled, it logs:

- SQL queries executed
- Parameter values (sanitized)
- Execution time in milliseconds
- Row counts returned

## Configuration

Query logging is controlled through [DatabaseOptions](../src/TechHub.Core/Configuration/DatabaseOptions.cs) in your `appsettings.json`:

```json
{
  "Database": {
    "Provider": "SQLite",
    "ConnectionString": "Data Source=.databases/sqlite/techhub.db",
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

## Output Format

Logged queries appear in the application logs:

```text
[Information] Query executed: 45ms, Rows=12
SQL: SELECT slug, title, date_epoch...
Params: collection="blogs", take=20, skip=0
```

## Usage

Query logging is automatically used by [DatabaseContentRepository](../src/TechHub.Infrastructure/Repositories/DatabaseContentRepository.cs) when `EnableQueryLogging` is `true`. No code changes are required.

To enable logging temporarily:

1. Edit `appsettings.json` or `appsettings.Development.json`
2. Set `Database:EnableQueryLogging` to `true`
3. Restart the application
4. Observe logs for query details

## Related

- [Database Configuration](database.md)
- [Testing Strategy](testing-strategy.md)
