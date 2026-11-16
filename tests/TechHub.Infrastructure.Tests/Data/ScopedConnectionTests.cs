using System.Data;
using Dapper;
using FluentAssertions;
using Npgsql;

namespace TechHub.Infrastructure.Tests.Data;

/// <summary>
/// The API registers a closed connection from the data source as its scoped <see cref="IDbConnection"/>
/// (<c>NpgsqlDataSource.CreateConnection()</c>). These tests pin the behaviour that registration relies on.
/// </summary>
public class ScopedConnectionTests : IClassFixture<DatabaseFixture<ScopedConnectionTests>>
{
    private readonly DatabaseFixture<ScopedConnectionTests> _fixture;

    public ScopedConnectionTests(DatabaseFixture<ScopedConnectionTests> fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);
        _fixture = fixture;
    }

    [Fact]
    public async Task Connection_IsClosedUntilUsed_AndDapperReleasesItAfterEachStatement()
    {
        // Arrange
        using var connection = _fixture.DataSource.CreateConnection();
        connection.State.Should().Be(ConnectionState.Closed);

        // Act
        var value = await connection.ExecuteScalarAsync<int>("SELECT 1");

        // Assert
        value.Should().Be(1);
        connection.State.Should().Be(ConnectionState.Closed);
    }

    [Fact]
    public async Task Query_WithArrayParameter_UsesNativeArrays()
    {
        // Arrange
        using var connection = _fixture.DataSource.CreateConnection();

        // Act
        var count = await connection.ExecuteScalarAsync<int>(
            "SELECT count(*) FROM generate_series(1, 5) AS n WHERE n = ANY(@Ids)",
            new { Ids = new long[] { 1, 3, 5 } });

        // Assert
        count.Should().Be(3);
    }

    [Fact]
    public async Task Query_WhenPoolIsExhausted_StopsWaitingOnCancellation()
    {
        // Arrange — a one-connection pool that is fully in use
        var connectionString = new NpgsqlConnectionStringBuilder(_fixture.ConnectionString)
        {
            MaxPoolSize = 1,
            Timeout = 30
        }.ConnectionString;
        await using var dataSource = NpgsqlDataSource.Create(connectionString);
        await using var held = await dataSource.OpenConnectionAsync(TestContext.Current.CancellationToken);
        using var connection = dataSource.CreateConnection();
        using var cts = new CancellationTokenSource();

        // Act
        var query = connection.ExecuteScalarAsync<int>(new CommandDefinition("SELECT 1", cancellationToken: cts.Token));
        await Task.Delay(200, TestContext.Current.CancellationToken);
        var stillWaiting = !query.IsCompleted;
        await cts.CancelAsync();
        var act = async () => await query.WaitAsync(TimeSpan.FromSeconds(10), TestContext.Current.CancellationToken);

        // Assert — the wait ends because of the cancellation, not the 30 second pool timeout
        stillWaiting.Should().BeTrue();
        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task OpenAsync_KeepsOnePhysicalConnectionUntilClosed()
    {
        // Arrange
        using var connection = _fixture.DataSource.CreateConnection();
        await connection.OpenAsync(TestContext.Current.CancellationToken);

        // Act — a temp table only survives when all statements run on the same physical connection
        await connection.ExecuteAsync("CREATE TEMP TABLE scoped_probe (id int)");
        await connection.ExecuteAsync("INSERT INTO scoped_probe VALUES (1)");
        var count = await connection.ExecuteScalarAsync<int>("SELECT count(*) FROM scoped_probe");

        // Assert
        count.Should().Be(1);
        connection.State.Should().Be(ConnectionState.Open);
    }
}
