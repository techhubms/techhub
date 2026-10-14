using Dapper;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Npgsql;
using TechHub.Infrastructure.Data;

namespace TechHub.Infrastructure.Tests.Data;

/// <summary>
/// Tests <see cref="MigrationRunner"/> against empty PostgreSQL databases so that the first run,
/// repeat runs, failures and concurrent start-ups can be observed from the <c>_migrations</c> table.
/// </summary>
public class MigrationRunnerTests : IClassFixture<MigrationDatabaseFixture>
{
    private const string MigrationResourcePrefix = "TechHub.Infrastructure.Data.Migrations.postgres.";

    private readonly MigrationDatabaseFixture _fixture;

    public MigrationRunnerTests(MigrationDatabaseFixture fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);
        _fixture = fixture;
    }

    // ── Constructor guards ─────────────────────────────────────────────────

    [Fact]
    public void Constructor_NullConnection_ThrowsArgumentNullException()
    {
        // Act
        var act = () => new MigrationRunner(null!, new PostgresDialect(), NullLogger<MigrationRunner>.Instance);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("connection");
    }

    [Fact]
    public void Constructor_NullDialect_ThrowsArgumentNullException()
    {
        // Arrange
        using var connection = new NpgsqlConnection();

        // Act
        var act = () => new MigrationRunner(connection, null!, NullLogger<MigrationRunner>.Instance);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("dialect");
    }

    [Fact]
    public void Constructor_NullLogger_ThrowsArgumentNullException()
    {
        // Arrange
        using var connection = new NpgsqlConnection();

        // Act
        var act = () => new MigrationRunner(connection, new PostgresDialect(), null!);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("logger");
    }

    // ── First run ──────────────────────────────────────────────────────────

    [Fact]
    public async Task RunMigrationsAsync_EmptyDatabase_RecordsEveryEmbeddedScript()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var connection = await OpenFreshDatabaseAsync(ct);
        var runner = CreateRunner(connection);

        // Act
        await runner.RunMigrationsAsync(ct);

        // Assert
        var recorded = await GetRecordedScriptNamesAsync(connection);
        recorded.Should().BeEquivalentTo(GetEmbeddedMigrationNames());
    }

    [Fact]
    public async Task RunMigrationsAsync_EmptyDatabase_RecordsScriptsInFilenameOrder()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var connection = await OpenFreshDatabaseAsync(ct);
        var runner = CreateRunner(connection);

        // Act
        await runner.RunMigrationsAsync(ct);

        // Assert
        var executionOrder = (await connection.QueryAsync<string>(
            "SELECT script_name FROM _migrations ORDER BY executed_at, script_name")).ToList();
        executionOrder.Should().Equal(executionOrder.OrderBy(name => name, StringComparer.Ordinal));
        executionOrder[0].Should().EndWith("001_initial_schema.sql");
    }

    [Fact]
    public async Task RunMigrationsAsync_EmptyDatabase_CreatesSchemaAndSeedsJobSettings()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var connection = await OpenFreshDatabaseAsync(ct);
        var runner = CreateRunner(connection);

        // Act
        await runner.RunMigrationsAsync(ct);

        // Assert
        string[] expectedTables =
        [
            "content_items", "content_tags_expanded", "content_reviews", "custom_page_data",
            "background_job_settings", "newsletter_subscribers", "newsletter_send_log"
        ];
        foreach (var table in expectedTables)
        {
            (await TableExistsAsync(connection, table)).Should().BeTrue($"table {table} should be created by the migrations");
        }

        var jobNames = await connection.QueryAsync<string>("SELECT job_name FROM background_job_settings");
        jobNames.Should().BeEquivalentTo("ContentProcessor", "RoundupGenerator", "Newsletter");
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task RunMigrationsAsync_ExistingDatabase_RemovesOnlyInertContentCleanupSetting(bool cleanupSettingExists)
    {
        // Arrange - defer the removal migration to reproduce an already-deployed database.
        const string RemovalScriptName = MigrationResourcePrefix + "048_remove_content_cleanup_job_setting.sql";
        var ct = TestContext.Current.CancellationToken;
        await using var connection = await OpenFreshDatabaseAsync(ct);
        await connection.ExecuteAsync(
            "CREATE TABLE _migrations (script_name TEXT PRIMARY KEY, executed_at TIMESTAMPTZ NOT NULL DEFAULT NOW())");
        await connection.ExecuteAsync(
            "INSERT INTO _migrations (script_name) VALUES (@Name)", new { Name = RemovalScriptName });
        await CreateRunner(connection).RunMigrationsAsync(ct);
        await connection.ExecuteAsync(
            "UPDATE background_job_settings SET enabled = TRUE, description = 'Customized setting' WHERE job_name IN ('ContentProcessor', 'ContentCleanup')");
        if (!cleanupSettingExists)
        {
            await connection.ExecuteAsync("DELETE FROM background_job_settings WHERE job_name = 'ContentCleanup'");
        }

        var settingsBefore = (await connection.QueryAsync<JobSetting>(
            "SELECT job_name AS JobName, enabled, description FROM background_job_settings WHERE job_name <> 'ContentCleanup' ORDER BY job_name")).ToList();
        await connection.ExecuteAsync(
            "DELETE FROM _migrations WHERE script_name = @Name", new { Name = RemovalScriptName });
        var migrationsBefore = await GetRecordedExecutionsAsync(connection);

        // Act
        await CreateRunner(connection).RunMigrationsAsync(ct);

        // Assert
        (await connection.QueryAsync<JobSetting>(
            "SELECT job_name AS JobName, enabled, description FROM background_job_settings ORDER BY job_name"))
            .Should().Equal(settingsBefore, "only the unused setting should be removed");
        var migrationsAfter = await GetRecordedExecutionsAsync(connection);
        migrationsAfter.Should().ContainSingle(row => row.ScriptName == RemovalScriptName);
        migrationsAfter.Where(row => row.ScriptName != RemovalScriptName)
            .Should().Equal(migrationsBefore, "historical migrations must not be re-executed");
    }

    // ── Repeat runs ────────────────────────────────────────────────────────

    [Fact]
    public async Task RunMigrationsAsync_SecondRun_ExecutesNothing()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var connection = await OpenFreshDatabaseAsync(ct);
        await CreateRunner(connection).RunMigrationsAsync(ct);
        var before = await GetRecordedExecutionsAsync(connection);

        // Act — a new runner instance simulates an application restart
        await CreateRunner(connection).RunMigrationsAsync(ct);

        // Assert
        var after = await GetRecordedExecutionsAsync(connection);
        after.Should().Equal(before, "no script may be re-executed or re-recorded");
    }

    [Fact]
    public async Task RunMigrationsAsync_SecondRun_PreservesDataChangedAfterFirstRun()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var connection = await OpenFreshDatabaseAsync(ct);
        await CreateRunner(connection).RunMigrationsAsync(ct);
        await connection.ExecuteAsync("UPDATE background_job_settings SET enabled = TRUE WHERE job_name = 'ContentProcessor'");

        // Act
        await CreateRunner(connection).RunMigrationsAsync(ct);

        // Assert
        var enabled = await connection.ExecuteScalarAsync<bool>(
            "SELECT enabled FROM background_job_settings WHERE job_name = 'ContentProcessor'");
        enabled.Should().BeTrue();
    }

    [Fact]
    public async Task RunMigrationsAsync_AllScriptsAlreadyRecorded_SkipsThemRegardlessOfCase()
    {
        // Arrange — track every script as executed without creating any schema
        var ct = TestContext.Current.CancellationToken;
        await using var connection = await OpenFreshDatabaseAsync(ct);
        await connection.ExecuteAsync(
            "CREATE TABLE _migrations (script_name TEXT PRIMARY KEY, executed_at TIMESTAMPTZ NOT NULL DEFAULT NOW())");
        foreach (var name in GetEmbeddedMigrationNames())
        {
            await connection.ExecuteAsync(
                "INSERT INTO _migrations (script_name) VALUES (@Name)", new { Name = name.ToUpperInvariant() });
        }

        // Act
        await CreateRunner(connection).RunMigrationsAsync(ct);

        // Assert
        (await TableExistsAsync(connection, "content_items")).Should().BeFalse("no script should have been executed");
        (await GetRecordedScriptNamesAsync(connection)).Should().HaveCount(GetEmbeddedMigrationNames().Length);
    }

    [Fact]
    public async Task RunMigrationsAsync_OneScriptMissingFromTracking_RunsOnlyThatScript()
    {
        // Arrange — 047 only uses ADD COLUMN IF NOT EXISTS, so it can safely be re-applied
        const string RepeatableScriptSuffix = "047_newsletter_subscriber_send_status.sql";
        var ct = TestContext.Current.CancellationToken;
        await using var connection = await OpenFreshDatabaseAsync(ct);
        await CreateRunner(connection).RunMigrationsAsync(ct);
        await connection.ExecuteAsync(
            "DELETE FROM _migrations WHERE script_name LIKE '%' || @Suffix", new { Suffix = RepeatableScriptSuffix });
        var before = await GetRecordedExecutionsAsync(connection);
        before.Should().HaveCount(GetEmbeddedMigrationNames().Length - 1);

        // Act
        await CreateRunner(connection).RunMigrationsAsync(ct);

        // Assert
        var after = await GetRecordedExecutionsAsync(connection);
        after.Should().HaveCount(GetEmbeddedMigrationNames().Length);
        after.Where(row => !row.ScriptName.EndsWith(RepeatableScriptSuffix, StringComparison.Ordinal))
            .Should().Equal(before, "scripts that were already tracked must not be touched");
    }

    // ── Failure handling ───────────────────────────────────────────────────

    [Fact]
    public async Task RunMigrationsAsync_FailingScript_ThrowsAndDoesNotRecordIt()
    {
        // Arrange — a conflicting content_items table makes the first script fail on its index statements
        var ct = TestContext.Current.CancellationToken;
        await using var connection = await OpenFreshDatabaseAsync(ct);
        await connection.ExecuteAsync("CREATE TABLE content_items (slug TEXT)");
        var runner = CreateRunner(connection);

        // Act
        var act = () => runner.RunMigrationsAsync(ct);

        // Assert
        await act.Should().ThrowAsync<PostgresException>();
        (await GetRecordedScriptNamesAsync(connection)).Should().BeEmpty("a failed script must not be tracked as executed");
        (await TableExistsAsync(connection, "custom_page_data")).Should().BeFalse("later scripts must not run after a failure");
    }

    [Fact]
    public async Task RunMigrationsAsync_AfterFailureIsFixed_RetriesAndCompletes()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var connection = await OpenFreshDatabaseAsync(ct);
        await connection.ExecuteAsync("CREATE TABLE content_items (slug TEXT)");
        var runner = CreateRunner(connection);
        var failingRun = () => runner.RunMigrationsAsync(ct);
        await failingRun.Should().ThrowAsync<PostgresException>();
        await connection.ExecuteAsync("DROP TABLE content_items CASCADE");

        // Act — also proves the migration lock was released after the failure
        await runner.RunMigrationsAsync(ct);

        // Assert
        (await GetRecordedScriptNamesAsync(connection)).Should().BeEquivalentTo(GetEmbeddedMigrationNames());
    }

    [Fact]
    public async Task RunMigrationsAsync_CancelledToken_ThrowsWithoutTouchingDatabase()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var connection = await OpenFreshDatabaseAsync(ct);
        var runner = CreateRunner(connection);
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        // Act
        var act = () => runner.RunMigrationsAsync(cts.Token);

        // Assert
        await act.Should().ThrowAsync<OperationCanceledException>();
        (await TableExistsAsync(connection, "_migrations")).Should().BeFalse();
    }

    // ── Concurrency ────────────────────────────────────────────────────────

    [Fact]
    public async Task RunMigrationsAsync_ConcurrentRunners_ApplyEachScriptExactlyOnce()
    {
        // Arrange — several application instances starting at the same time against one database
        const int RunnerCount = 4;
        var ct = TestContext.Current.CancellationToken;
        var connectionString = await _fixture.CreateEmptyDatabaseAsync(ct);
        var connections = new List<NpgsqlConnection>();
        try
        {
            for (var i = 0; i < RunnerCount; i++)
            {
                connections.Add(await MigrationDatabaseFixture.OpenAsync(connectionString, ct));
            }

            // Act — a duplicate run would violate the _migrations primary key or fail on non-idempotent scripts
            await Task.WhenAll(connections.Select(connection => CreateRunner(connection).RunMigrationsAsync(ct)));

            // Assert
            var recorded = await GetRecordedScriptNamesAsync(connections[0]);
            recorded.Should().BeEquivalentTo(GetEmbeddedMigrationNames());
            recorded.Should().OnlyHaveUniqueItems();
        }
        finally
        {
            foreach (var connection in connections)
            {
                await connection.DisposeAsync();
            }
        }
    }

    // ── Helpers ────────────────────────────────────────────────────────────

    private static MigrationRunner CreateRunner(NpgsqlConnection connection) =>
        new(connection, new PostgresDialect(), NullLogger<MigrationRunner>.Instance);

    private async Task<NpgsqlConnection> OpenFreshDatabaseAsync(CancellationToken ct)
    {
        var connectionString = await _fixture.CreateEmptyDatabaseAsync(ct);
        return await MigrationDatabaseFixture.OpenAsync(connectionString, ct);
    }

    private static string[] GetEmbeddedMigrationNames() =>
        typeof(MigrationRunner).Assembly.GetManifestResourceNames()
            .Where(name => name.StartsWith(MigrationResourcePrefix, StringComparison.Ordinal)
                           && name.EndsWith(".sql", StringComparison.Ordinal))
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToArray();

    private static async Task<List<string>> GetRecordedScriptNamesAsync(NpgsqlConnection connection) =>
        (await connection.QueryAsync<string>("SELECT script_name FROM _migrations ORDER BY script_name")).ToList();

    private static async Task<List<RecordedExecution>> GetRecordedExecutionsAsync(NpgsqlConnection connection) =>
        (await connection.QueryAsync<RecordedExecution>(
            "SELECT script_name AS ScriptName, executed_at::text AS ExecutedAt FROM _migrations ORDER BY script_name")).ToList();

    private static async Task<bool> TableExistsAsync(NpgsqlConnection connection, string tableName) =>
        await connection.ExecuteScalarAsync<bool>(
            "SELECT to_regclass(@Name) IS NOT NULL", new { Name = $"public.{tableName}" });

    private sealed record RecordedExecution(string ScriptName, string ExecutedAt);

    private sealed record JobSetting(string JobName, bool Enabled, string Description);
}
