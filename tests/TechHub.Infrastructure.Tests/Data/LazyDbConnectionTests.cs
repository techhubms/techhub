using System.Data;
using Dapper;
using FluentAssertions;
using TechHub.Infrastructure.Data;

namespace TechHub.Infrastructure.Tests.Data;

public class LazyDbConnectionTests : IClassFixture<DatabaseFixture<LazyDbConnectionTests>>
{
    private readonly DatabaseFixture<LazyDbConnectionTests> _fixture;

    public LazyDbConnectionTests(DatabaseFixture<LazyDbConnectionTests> fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);
        _fixture = fixture;
    }

    [Fact]
    public void Constructor_NullDataSource_ThrowsArgumentNullException()
    {
        // Act
        var act = () => new LazyDbConnection(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("dataSource");
    }

    [Fact]
    public void NewConnection_DoesNotAcquirePooledConnection()
    {
        // Act
        using var connection = new LazyDbConnection(_fixture.DataSource);

        // Assert
        connection.IsAcquired.Should().BeFalse();
        connection.ConnectionString.Should().Be(_fixture.DataSource.ConnectionString);
    }

    [Fact]
    public async Task FirstQuery_AcquiresConnectionAndKeepsItOpenForSessionState()
    {
        // Arrange
        using var connection = new LazyDbConnection(_fixture.DataSource);

        // Act — a temp table only survives when both statements run on the same physical connection
        await connection.ExecuteAsync("CREATE TEMP TABLE lazy_probe (id int)");
        await connection.ExecuteAsync("INSERT INTO lazy_probe VALUES (1)");
        var count = await connection.ExecuteScalarAsync<int>("SELECT count(*) FROM lazy_probe");

        // Assert
        connection.IsAcquired.Should().BeTrue();
        connection.State.Should().Be(ConnectionState.Open);
        count.Should().Be(1);
    }

    [Fact]
    public async Task BeginTransaction_OnUnusedConnection_OpensItAndSupportsCommit()
    {
        // Arrange
        using var connection = new LazyDbConnection(_fixture.DataSource);

        // Act
        using var transaction = connection.BeginTransaction();
        var result = await connection.ExecuteScalarAsync<int>("SELECT 1", transaction: transaction);
        transaction.Commit();

        // Assert
        result.Should().Be(1);
        connection.IsAcquired.Should().BeTrue();
    }

    [Fact]
    public async Task Close_ReleasesConnectionAndNextUseReacquiresOne()
    {
        // Arrange
        using var connection = new LazyDbConnection(_fixture.DataSource);
        await connection.ExecuteScalarAsync<int>("SELECT 1");

        // Act
        connection.Close();
        var acquiredAfterClose = connection.IsAcquired;
        var value = await connection.ExecuteScalarAsync<int>("SELECT 2");

        // Assert
        acquiredAfterClose.Should().BeFalse();
        value.Should().Be(2);
        connection.IsAcquired.Should().BeTrue();
    }
}
