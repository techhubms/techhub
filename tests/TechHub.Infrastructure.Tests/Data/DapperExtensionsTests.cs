using System.Globalization;
using Dapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Npgsql;
using TechHub.Infrastructure.Data;

namespace TechHub.Infrastructure.Tests.Data;

/// <summary>
/// Tests the query logging helpers in <see cref="DapperExtensions"/> against a real PostgreSQL database.
/// The slow-query test relies on <c>pg_sleep</c> because the threshold is measured with a real stopwatch.
/// </summary>
public class DapperExtensionsTests : IClassFixture<DatabaseFixture<DapperExtensionsTests>>
{
    private readonly DatabaseFixture<DapperExtensionsTests> _fixture;

    public DapperExtensionsTests(DatabaseFixture<DapperExtensionsTests> fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);
        _fixture = fixture;
    }

    // ── QueryWithLoggingAsync: logging switched off ────────────────────────

    [Fact]
    public async Task QueryWithLoggingAsync_LoggingDisabled_ReturnsRowsAndLogsNothing()
    {
        // Arrange
        var logger = new RecordingLogger();
        var command = new CommandDefinition("SELECT 41 + 1 AS value", cancellationToken: TestContext.Current.CancellationToken);

        // Act
        var rows = await _fixture.Connection.QueryWithLoggingAsync<int>(command, logger, enableLogging: false);

        // Assert
        rows.Should().Equal(42);
        logger.Entries.Should().BeEmpty();
    }

    [Fact]
    public async Task QueryWithLoggingAsync_NoLogger_ReturnsRows()
    {
        // Arrange
        var command = new CommandDefinition("SELECT 41 + 1 AS value", cancellationToken: TestContext.Current.CancellationToken);

        // Act
        var rows = await _fixture.Connection.QueryWithLoggingAsync<int>(command, logger: null, enableLogging: true);

        // Assert
        rows.Should().Equal(42);
    }

    // ── QueryWithLoggingAsync: fast queries ────────────────────────────────

    [Fact]
    public async Task QueryWithLoggingAsync_FastQuery_LogsInformationWithSqlAndRowCount()
    {
        // Arrange
        var logger = new RecordingLogger();
        var command = new CommandDefinition("SELECT generate_series(1, 3) AS value", cancellationToken: TestContext.Current.CancellationToken);

        // Act
        var rows = await _fixture.Connection.QueryWithLoggingAsync<int>(command, logger, enableLogging: true);

        // Assert
        rows.Should().Equal(1, 2, 3);
        var entry = logger.Entries.Should().ContainSingle().Subject;
        entry.Level.Should().Be(LogLevel.Information);
        entry.Message.Should().Contain("Query executed");
        entry.Message.Should().Contain("Rows=3");
        entry.Message.Should().Contain("SELECT generate_series(1, 3) AS value");
        entry.Message.Should().Contain("Params: (none)");
    }

    [Fact]
    public async Task QueryWithLoggingAsync_FastQuery_DoesNotLogWarningsOrExplainPlan()
    {
        // Arrange
        var logger = new RecordingLogger();
        var command = new CommandDefinition("SELECT 1 AS value", cancellationToken: TestContext.Current.CancellationToken);

        // Act
        await _fixture.Connection.QueryWithLoggingAsync<int>(command, logger, enableLogging: true);

        // Assert
        logger.Entries.Should().NotContain(e => e.Level >= LogLevel.Warning);
    }

    [Fact]
    public async Task QueryWithLoggingAsync_SensitiveParameterNames_AreMaskedInLog()
    {
        // Arrange
        var logger = new RecordingLogger();
        var command = new CommandDefinition(
            "SELECT @Name AS value",
            new { Name = "alice", Password = "hunter2", ClientSecret = "s3cr3t", AccessToken = "tok-123" },
            cancellationToken: TestContext.Current.CancellationToken);

        // Act
        await _fixture.Connection.QueryWithLoggingAsync<string>(command, logger, enableLogging: true);

        // Assert
        var message = logger.Entries.Should().ContainSingle().Subject.Message;
        message.Should().Contain("Name=\"alice\"");
        message.Should().Contain("Password=***");
        message.Should().Contain("ClientSecret=***");
        message.Should().Contain("AccessToken=***");
        message.Should().NotContain("hunter2").And.NotContain("s3cr3t").And.NotContain("tok-123");
    }

    [Fact]
    public async Task QueryWithLoggingAsync_LongStringParameter_IsTruncatedInLog()
    {
        // Arrange
        var logger = new RecordingLogger();
        var command = new CommandDefinition(
            "SELECT @Name AS value",
            new { Name = new string('x', 150) },
            cancellationToken: TestContext.Current.CancellationToken);

        // Act
        await _fixture.Connection.QueryWithLoggingAsync<string>(command, logger, enableLogging: true);

        // Assert
        var message = logger.Entries.Should().ContainSingle().Subject.Message;
        message.Should().Contain(new string('x', 97) + "...\"");
        message.Should().NotContain(new string('x', 98));
    }

    [Fact]
    public async Task QueryWithLoggingAsync_NullParameterValue_IsLoggedAsNull()
    {
        // Arrange
        var logger = new RecordingLogger();
        var command = new CommandDefinition(
            "SELECT CAST(@Value AS text) AS value",
            new { Value = (string?)null },
            cancellationToken: TestContext.Current.CancellationToken);

        // Act
        await _fixture.Connection.QueryWithLoggingAsync<string?>(command, logger, enableLogging: true);

        // Assert
        logger.Entries.Should().ContainSingle().Subject.Message.Should().Contain("Value=null");
    }

    // ── QueryWithLoggingAsync: slow queries ────────────────────────────────

    [Fact]
    public async Task QueryWithLoggingAsync_QuerySlowerThanThreshold_LogsSlowQueryWarningAndExplainPlan()
    {
        // Arrange — sleep just past the threshold; EXPLAIN ANALYZE re-runs the query, so the sleep is paid twice
        var sleepSeconds = (DapperExtensions.SlowQueryThresholdMs + 200) / 1000.0;
        var sql = string.Create(CultureInfo.InvariantCulture, $"SELECT 1 AS value FROM pg_sleep({sleepSeconds})");
        var logger = new RecordingLogger();
        var command = new CommandDefinition(sql, cancellationToken: TestContext.Current.CancellationToken);

        // Act
        var rows = await _fixture.Connection.QueryWithLoggingAsync<int>(command, logger, enableLogging: true);

        // Assert
        rows.Should().Equal(1);
        var warnings = logger.Entries.Where(e => e.Level == LogLevel.Warning).Select(e => e.Message).ToList();
        warnings.Should().HaveCount(2);
        warnings[0].Should().StartWith("SLOW QUERY:").And.Contain("Rows=1").And.Contain("pg_sleep");
        warnings[1].Should().Contain("EXPLAIN plan for slow query");
        logger.Entries.Should().NotContain(e => e.Level == LogLevel.Information);
    }

    // ── QueryWithLoggingAsync: failures ────────────────────────────────────

    [Fact]
    public async Task QueryWithLoggingAsync_FailingQuery_LogsErrorAndRethrows()
    {
        // Arrange
        var logger = new RecordingLogger();
        var command = new CommandDefinition("SELECT * FROM table_that_does_not_exist", cancellationToken: TestContext.Current.CancellationToken);

        // Act
        var act = () => _fixture.Connection.QueryWithLoggingAsync<int>(command, logger, enableLogging: true);

        // Assert
        var exception = await act.Should().ThrowAsync<PostgresException>();
        var entry = logger.Entries.Should().ContainSingle().Subject;
        entry.Level.Should().Be(LogLevel.Error);
        entry.Message.Should().Contain("Query failed after").And.Contain("table_that_does_not_exist");
        entry.Exception.Should().BeSameAs(exception.Which);
    }

    // ── QueryMultipleWithLoggingAsync ──────────────────────────────────────

    [Fact]
    public async Task QueryMultipleWithLoggingAsync_LoggingDisabled_ReturnsGridAndLogsNothing()
    {
        // Arrange
        var logger = new RecordingLogger();
        var command = new CommandDefinition("SELECT 1 AS a; SELECT 2 AS b", cancellationToken: TestContext.Current.CancellationToken);

        // Act
        using var grid = await _fixture.Connection.QueryMultipleWithLoggingAsync(command, logger, enableLogging: false);
        var first = await grid.ReadAsync<int>();
        var second = await grid.ReadAsync<int>();

        // Assert
        first.Should().Equal(1);
        second.Should().Equal(2);
        logger.Entries.Should().BeEmpty();
    }

    [Fact]
    public async Task QueryMultipleWithLoggingAsync_LoggingEnabled_LogsInformationWithoutRowCount()
    {
        // Arrange
        var logger = new RecordingLogger();
        var command = new CommandDefinition("SELECT 1 AS a; SELECT 2 AS b", cancellationToken: TestContext.Current.CancellationToken);

        // Act
        using var grid = await _fixture.Connection.QueryMultipleWithLoggingAsync(command, logger, enableLogging: true);
        var first = await grid.ReadAsync<int>();
        var second = await grid.ReadAsync<int>();

        // Assert
        first.Should().Equal(1);
        second.Should().Equal(2);
        var entry = logger.Entries.Should().ContainSingle().Subject;
        entry.Level.Should().Be(LogLevel.Information);
        entry.Message.Should().Contain("Query executed").And.NotContain("Rows=");
    }

    [Fact]
    public async Task QueryMultipleWithLoggingAsync_FailingQuery_LogsErrorAndRethrows()
    {
        // Arrange
        var logger = new RecordingLogger();
        var command = new CommandDefinition("SELECT * FROM table_that_does_not_exist", cancellationToken: TestContext.Current.CancellationToken);

        // Act
        var act = () => _fixture.Connection.QueryMultipleWithLoggingAsync(command, logger, enableLogging: true);

        // Assert
        await act.Should().ThrowAsync<PostgresException>();
        var entry = logger.Entries.Should().ContainSingle().Subject;
        entry.Level.Should().Be(LogLevel.Error);
        entry.Message.Should().Contain("Multi-query failed after");
    }

    private sealed class RecordingLogger : ILogger
    {
        public List<(LogLevel Level, string Message, Exception? Exception)> Entries { get; } = [];

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter) =>
            Entries.Add((logLevel, formatter(state, exception), exception));
    }
}
