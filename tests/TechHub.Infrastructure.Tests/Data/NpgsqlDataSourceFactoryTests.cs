using FluentAssertions;
using Npgsql;
using TechHub.Infrastructure.Data;

namespace TechHub.Infrastructure.Tests.Data;

/// <summary>
/// Only the password-based path is tested: the Entra ID path needs a real managed identity token.
/// </summary>
public class NpgsqlDataSourceFactoryTests
{
    [Fact]
    public void Create_PasswordAuth_BuildsDataSourceForConnectionString()
    {
        // Act
        using var dataSource = NpgsqlDataSourceFactory.Create("Host=db.example.com;Database=techhub;Username=app;Password=secret");

        // Assert
        var builder = new NpgsqlConnectionStringBuilder(dataSource.ConnectionString);
        builder.Host.Should().Be("db.example.com");
        builder.Database.Should().Be("techhub");
        builder.Username.Should().Be("app");
    }

    [Fact]
    public void Create_InvalidConnectionString_Throws()
    {
        // Act
        var act = () => NpgsqlDataSourceFactory.Create("this is not a connection string");

        // Assert
        act.Should().Throw<ArgumentException>();
    }
}
