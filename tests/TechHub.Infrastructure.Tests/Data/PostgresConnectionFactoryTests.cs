using System.Data;
using Dapper;
using FluentAssertions;
using TechHub.Infrastructure.Data;

namespace TechHub.Infrastructure.Tests.Data;

public class PostgresConnectionFactoryTests : IClassFixture<DatabaseFixture<PostgresConnectionFactoryTests>>
{
    private readonly PostgresConnectionFactory _factory;

    public PostgresConnectionFactoryTests(DatabaseFixture<PostgresConnectionFactoryTests> fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);
        _factory = new PostgresConnectionFactory(fixture.DataSource);
    }

    [Fact]
    public void Constructor_NullDataSource_ThrowsArgumentNullException()
    {
        // Act
        var act = () => new PostgresConnectionFactory(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("dataSource");
    }

    [Fact]
    public void CreateConnection_ReturnsOpenConnectionThatCanRunQueries()
    {
        // Act
        using var connection = _factory.CreateConnection();

        // Assert
        connection.State.Should().Be(ConnectionState.Open);
        connection.ExecuteScalar<int>("SELECT 1").Should().Be(1);
    }

    [Fact]
    public async Task CreateConnectionAsync_ReturnsOpenConnectionToTheConfiguredDatabase()
    {
        // Act
        using var connection = await _factory.CreateConnectionAsync(TestContext.Current.CancellationToken);

        // Assert
        connection.State.Should().Be(ConnectionState.Open);
        (await connection.ExecuteScalarAsync<string>("SELECT current_database()")).Should().Be("techhub_test");
    }

    [Fact]
    public async Task CreateConnectionAsync_CalledTwice_ReturnsIndependentConnections()
    {
        // Arrange
        var first = await _factory.CreateConnectionAsync(TestContext.Current.CancellationToken);
        using var second = await _factory.CreateConnectionAsync(TestContext.Current.CancellationToken);

        // Act
        first.Dispose();

        // Assert
        first.Should().NotBeSameAs(second);
        first.State.Should().Be(ConnectionState.Closed);
        second.State.Should().Be(ConnectionState.Open);
        (await second.ExecuteScalarAsync<int>("SELECT 1")).Should().Be(1);
    }

    [Fact]
    public async Task CreateConnectionAsync_AfterConnectionDisposed_ReturnsUsableConnection()
    {
        // Arrange
        (await _factory.CreateConnectionAsync(TestContext.Current.CancellationToken)).Dispose();

        // Act
        using var connection = await _factory.CreateConnectionAsync(TestContext.Current.CancellationToken);

        // Assert
        (await connection.ExecuteScalarAsync<int>("SELECT 1")).Should().Be(1);
    }

    [Fact]
    public async Task CreateConnectionAsync_ConcurrentCalls_AllReturnOpenConnections()
    {
        // Act
        var created = await Task.WhenAll(
            Enumerable.Range(0, 5).Select(_ => _factory.CreateConnectionAsync(TestContext.Current.CancellationToken)));
        using var first = created[0];
        using var second = created[1];
        using var third = created[2];
        using var fourth = created[3];
        using var fifth = created[4];

        // Assert
        created.Should().OnlyContain(c => c.State == ConnectionState.Open);
        created.Distinct().Should().HaveCount(5);
    }
}
