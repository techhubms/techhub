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
        recorded.Should().BeEquivalentTo(GetEmbeddedMigrationFileNames());
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
        const string RemovalScriptName = "048_remove_content_cleanup_job_setting.sql";
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
    public async Task RunMigrationsAsync_ReplayingOriginalCatalogSeed_PreservesCatalogAndCuratedParent()
    {
        const string SeedScript = "050_product_catalog.sql";
        const string CountSql = """
            SELECT (SELECT count(*) FROM products) + (SELECT count(*) FROM product_aliases)
                 + (SELECT count(*) FROM product_features) + (SELECT count(*) FROM ignored_product_names)
            """;
        var ct = TestContext.Current.CancellationToken;
        await using var connection = await OpenFreshDatabaseAsync(ct);

        // Replay the seed before later migrations rename and reclassify its entries.
        await connection.ExecuteAsync(
            "CREATE TABLE _migrations (script_name TEXT PRIMARY KEY, executed_at TIMESTAMPTZ NOT NULL DEFAULT NOW())");
        var laterScripts = GetEmbeddedMigrationFileNames()
            .Where(name => string.CompareOrdinal(name, SeedScript) > 0)
            .ToArray();
        await connection.ExecuteAsync("INSERT INTO _migrations (script_name) SELECT unnest(@Names)", new { Names = laterScripts });
        var runner = CreateRunner(connection);
        await runner.RunMigrationsAsync(ct);
        var parentId = await connection.ExecuteScalarAsync<long>(
            "INSERT INTO products (name) VALUES ('A parent chosen in the admin') RETURNING id");
        await connection.ExecuteAsync("UPDATE products SET parent_product_id = @Id WHERE name = 'Azure Batch'", new { Id = parentId });
        var before = await connection.ExecuteScalarAsync<long>(CountSql);

        await connection.ExecuteAsync("DELETE FROM _migrations WHERE script_name = @Name", new { Name = SeedScript });
        await runner.RunMigrationsAsync(ct);

        (await connection.ExecuteScalarAsync<long>(CountSql)).Should().Be(before);
        (await connection.ExecuteScalarAsync<long?>("SELECT parent_product_id FROM products WHERE name = 'Azure Batch'"))
            .Should().Be(parentId);
    }

    [Fact]
    public async Task RunMigrationsAsync_ReviewedCatalog_ResolvesNamesWithoutOverwritingCuration()
    {
        // Arrange
        const string ReviewScript = "063_reviewed_catalog_entries.sql";
        var ct = TestContext.Current.CancellationToken;
        await using var connection = await OpenFreshDatabaseAsync(ct);
        await connection.ExecuteAsync("""
            CREATE TABLE _migrations (script_name TEXT PRIMARY KEY, executed_at TIMESTAMPTZ NOT NULL DEFAULT NOW());
            INSERT INTO _migrations (script_name) VALUES (@Name);
            """, new { Name = ReviewScript });
        var runner = CreateRunner(connection);
        await runner.RunMigrationsAsync(ct);
        await connection.ExecuteAsync("""
            INSERT INTO product_aliases (product_id, alias)
            SELECT id, 'Azure Speech' FROM products WHERE name = 'Azure AI Speech';
            INSERT INTO product_features (product_id, name, description)
            SELECT id, 'Native AOT', 'Curated publishing notes' FROM products WHERE name = '.NET';
            INSERT INTO content_items
                (slug, collection_name, title, content, excerpt, date_epoch, primary_section_name,
                 external_url, author, feed_name, tags_csv, content_hash)
            SELECT 'reviewed-entry', 'news', 'Title', 'Text', '', 1750000000, 'azure',
                   'https://example.com/reviewed-entry', 'Author', name, '', 'hash'
            FROM rss_feed_configs ORDER BY name LIMIT 1;
            INSERT INTO content_item_products (collection_name, item_slug, product_name, position)
            VALUES ('news', 'reviewed-entry', 'Azure DocumentDB', 0);
            INSERT INTO content_timeline_events
                (collection_name, item_slug, kind, product, description, event_date, date_precision, date_source,
                 extracted_from, product_id)
            SELECT 'news', 'reviewed-entry', 'release', 'Azure DocumentDB', 'Existing event',
                   DATE '2026-11-18', 'day', 'explicit', src, CASE WHEN src = 'catalog' THEN p.id END
            FROM products p CROSS JOIN (VALUES ('catalog'), ('source')) AS s(src)
            WHERE p.name = 'Microsoft Azure';
            DELETE FROM _migrations WHERE script_name = @Name;
            """, new { Name = ReviewScript });
        (await connection.ExecuteScalarAsync<long?>("SELECT find_product_id('Azure DocumentDB')")).Should().BeNull();

        // Act
        await runner.RunMigrationsAsync(ct);

        // Assert
        var linkedProduct = await connection.ExecuteScalarAsync<string>("""
            SELECT p.name FROM content_item_products l JOIN products p ON p.id = l.product_id
            WHERE l.item_slug = 'reviewed-entry' AND l.product_name = 'Azure DocumentDB'
            """);
        linkedProduct.Should().Be("Azure DocumentDB");
        (await connection.QueryAsync<string>("""
            SELECT p.name FROM content_timeline_events e JOIN products p ON p.id = e.product_id
            WHERE e.item_slug = 'reviewed-entry' AND e.product = 'Azure DocumentDB'
            ORDER BY e.extracted_from
            """)).Should().Equal("Microsoft Azure", "Azure DocumentDB");
        (await connection.ExecuteScalarAsync<string>("""
            SELECT p.name FROM products p WHERE p.id = find_product_id('Azure Speech')
            """)).Should().Be("Azure AI Speech");
        (await connection.ExecuteScalarAsync<string>("""
            SELECT f.description FROM resolve_event_catalog('.NET', 'NativeAOT') r
            JOIN product_features f ON f.id = r.out_feature_id
            """)).Should().Be("Curated publishing notes");
        (await connection.ExecuteScalarAsync<string>("""
            SELECT p.name || ' / ' || f.name
            FROM resolve_event_catalog('SQL database in Microsoft Fabric', NULL) r
            JOIN products p ON p.id = r.out_product_id
            JOIN product_features f ON f.id = r.out_feature_id
            """)).Should().Be("Microsoft Fabric / SQL database");
        (await connection.ExecuteScalarAsync<bool>("""
            SELECT EXISTS (SELECT 1 FROM product_aliases WHERE lower(alias) = 'documentdb')
            """)).Should().BeFalse();
    }

    [Fact]
    public async Task RunMigrationsAsync_CatalogFollowUp_PreservesAspireIdentityAndResolvesReviewedNames()
    {
        const string FollowUpScript = "064_catalog_review_follow_up.sql";
        var ct = TestContext.Current.CancellationToken;
        await using var connection = await OpenFreshDatabaseAsync(ct);
        await connection.ExecuteAsync("""
            CREATE TABLE _migrations (script_name TEXT PRIMARY KEY, executed_at TIMESTAMPTZ NOT NULL DEFAULT NOW());
            INSERT INTO _migrations (script_name) VALUES (@Name);
            """, new { Name = FollowUpScript });
        var runner = CreateRunner(connection);
        await runner.RunMigrationsAsync(ct);
        var aspireId = await connection.QuerySingleAsync<long>("SELECT id FROM products WHERE name = '.NET Aspire'");
        await connection.ExecuteAsync("DELETE FROM _migrations WHERE script_name = @Name", new { Name = FollowUpScript });

        await runner.RunMigrationsAsync(ct);

        (await connection.QuerySingleAsync<long>("SELECT id FROM products WHERE name = 'Aspire'")).Should().Be(aspireId);
        (await connection.ExecuteScalarAsync<long?>("SELECT find_product_id('.NET Aspire')")).Should().Be(aspireId);
        (await connection.QuerySingleAsync<long?>("SELECT parent_product_id FROM products WHERE name = 'Aspire CLI'"))
            .Should().Be(aspireId);
        (await connection.QueryAsync<string>("""
            SELECT p.name
            FROM (VALUES (1, 'Azure AD Graph API'), (2, 'Microsoft Graph'), (3, 'Model Router Auto Evaluation')) v (position, name)
            JOIN products p ON p.id = find_product_id(v.name)
            ORDER BY v.position
            """)).Should().Equal("Azure AD Graph API", "Microsoft Graph", "Model Router Auto Evaluation toolkit");
        (await connection.QueryAsync<string>("""
            SELECT p.name || ' / ' || f.name
            FROM (VALUES
                (1, 'Azure Functions', 'Durable Functions'),
                (2, 'GitHub Copilot', 'Usage-based billing'),
                (3, 'Microsoft Fabric', 'Database Agents'),
                (4, 'Fabric HorizonDB', NULL),
                (5, 'Microsoft Foundry', 'Insights in Foundry'),
                (6, 'Microsoft Foundry', 'Foundry Toolboxes'),
                (7, '.NET Aspire', 'Aspire Dashboard')
            ) v (position, product, feature)
            CROSS JOIN LATERAL resolve_event_catalog(v.product, v.feature) r
            JOIN products p ON p.id = r.out_product_id
            JOIN product_features f ON f.id = r.out_feature_id
            ORDER BY v.position
            """)).Should().Equal("Azure Functions / Durable Functions", "GitHub Copilot / Token-based billing",
                "Microsoft Fabric / Database Agents", "Microsoft Fabric / Fabric HorizonDB", "Microsoft Foundry / Insights in Foundry",
                "Microsoft Foundry / Foundry Toolbox", "Aspire / Aspire Dashboard");
        (await connection.ExecuteScalarAsync<long?>("SELECT find_feature_id_by_name('Insights')")).Should().BeNull();
    }

    [Fact]
    public async Task RunMigrationsAsync_CatalogMatchingOptimization_PreservesResolutionAndCatalogData()
    {
        // Arrange
        const string OptimizationScript = "062_catalog_matching_evaluation.sql";
        const string ResolutionSql = """
            WITH pairs AS (
                SELECT p.name AS product, f.name AS feature
                FROM products p
                LEFT JOIN product_features f ON f.product_id = p.id
                UNION
                SELECT p.name, 'unmatched capability' FROM products p
                UNION
                SELECT p.name, f.name || ' experience'
                FROM products p JOIN product_features f ON f.product_id = p.id
                UNION
                SELECT a.alias, NULL FROM product_aliases a
                UNION
                SELECT 'Microsoft ' || p.name || ' (Preview)', NULL FROM products p
                UNION
                SELECT p.name || ' 2026', NULL FROM products p
                UNION
                SELECT 'Unknown product', 'Unknown feature'
            )
            SELECT COALESCE(jsonb_agg(to_jsonb(r) ORDER BY r.product, r.feature), '[]'::jsonb)::text
            FROM (
                SELECT p.*, c.out_product_id, c.out_feature_id
                FROM pairs p CROSS JOIN LATERAL resolve_event_catalog(p.product, p.feature) c
            ) r
            """;
        const string CatalogSql = """
            SELECT jsonb_build_array(
                (SELECT jsonb_agg(to_jsonb(p) - 'match_key' ORDER BY id) FROM products p),
                (SELECT jsonb_agg(to_jsonb(a) - 'match_key' ORDER BY id) FROM product_aliases a),
                (SELECT jsonb_agg(to_jsonb(f) - 'match_key' ORDER BY id) FROM product_features f),
                (SELECT jsonb_agg(to_jsonb(a) - 'match_key' ORDER BY id) FROM product_feature_aliases a))::text
            """;
        var ct = TestContext.Current.CancellationToken;
        await using var connection = await OpenFreshDatabaseAsync(ct);
        await connection.ExecuteAsync(
            "CREATE TABLE _migrations (script_name TEXT PRIMARY KEY, executed_at TIMESTAMPTZ NOT NULL DEFAULT NOW())");
        await connection.ExecuteAsync(
            "INSERT INTO _migrations (script_name) VALUES (@Name)", new { Name = OptimizationScript });
        await CreateRunner(connection).RunMigrationsAsync(ct);
        var resolutionBefore = await connection.ExecuteScalarAsync<string>(ResolutionSql);
        var catalogBefore = await connection.ExecuteScalarAsync<string>(CatalogSql);
        await connection.ExecuteAsync(
            "DELETE FROM _migrations WHERE script_name = @Name", new { Name = OptimizationScript });

        // Act
        await CreateRunner(connection).RunMigrationsAsync(ct);

        // Assert
        (await connection.ExecuteScalarAsync<string>(ResolutionSql)).Should().Be(resolutionBefore);
        (await connection.ExecuteScalarAsync<string>(CatalogSql)).Should().Be(catalogBefore);
    }

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

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task RunMigrationsAsync_AllScriptsAlreadyRecorded_SkipsThemRegardlessOfCase(bool legacyResourceNames)
    {
        // Arrange — track every script as executed without creating any schema
        var ct = TestContext.Current.CancellationToken;
        await using var connection = await OpenFreshDatabaseAsync(ct);
        await connection.ExecuteAsync(
            "CREATE TABLE _migrations (script_name TEXT PRIMARY KEY, executed_at TIMESTAMPTZ NOT NULL DEFAULT NOW())");
        foreach (var fileName in GetEmbeddedMigrationFileNames())
        {
            var name = legacyResourceNames ? MigrationResourcePrefix + fileName : fileName;
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
    public async Task RunMigrationsAsync_LegacyTrackingRows_RecordsOnlyPendingScriptByFilename()
    {
        // Arrange
        const string PendingScriptName = "048_remove_content_cleanup_job_setting.sql";
        var ct = TestContext.Current.CancellationToken;
        await using var connection = await OpenFreshDatabaseAsync(ct);
        await CreateRunner(connection).RunMigrationsAsync(ct);
        await connection.ExecuteAsync(
            "UPDATE _migrations SET script_name = @Prefix || script_name",
            new { Prefix = MigrationResourcePrefix });
        await connection.ExecuteAsync(
            "DELETE FROM _migrations WHERE script_name = @Name",
            new { Name = MigrationResourcePrefix + PendingScriptName });
        await connection.ExecuteAsync(
            "UPDATE background_job_settings SET enabled = TRUE WHERE job_name = 'ContentProcessor'");
        var before = await GetRecordedExecutionsAsync(connection);

        // Act
        await CreateRunner(connection).RunMigrationsAsync(ct);
        await CreateRunner(connection).RunMigrationsAsync(ct);

        // Assert
        var after = await GetRecordedExecutionsAsync(connection);
        after.Should().ContainSingle(row => row.ScriptName == PendingScriptName);
        after.Where(row => row.ScriptName != PendingScriptName)
            .Should().Equal(before, "legacy tracking rows must not be rewritten or re-executed");
        (await connection.ExecuteScalarAsync<bool>(
            "SELECT enabled FROM background_job_settings WHERE job_name = 'ContentProcessor'")).Should().BeTrue();
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
        (await GetRecordedScriptNamesAsync(connection)).Should().BeEquivalentTo(GetEmbeddedMigrationFileNames());
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
            recorded.Should().BeEquivalentTo(GetEmbeddedMigrationFileNames());
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

    private static string[] GetEmbeddedMigrationFileNames() =>
        GetEmbeddedMigrationNames().Select(name => name[MigrationResourcePrefix.Length..]).ToArray();

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
