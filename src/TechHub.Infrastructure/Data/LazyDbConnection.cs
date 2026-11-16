using System.Data;
using System.Data.Common;
using Npgsql;

namespace TechHub.Infrastructure.Data;

/// <summary>
/// A scoped connection that takes a connection from the <see cref="NpgsqlDataSource"/> pool on first use
/// instead of when it is resolved. Requests that never touch the database (cache hits, callers waiting
/// on a single-flight cache fill) therefore never hold a pooled connection. Once opened, the connection
/// stays open until it is closed or the scope is disposed, so session state and transactions behave as
/// they do for an eagerly opened connection.
/// </summary>
public sealed class LazyDbConnection : DbConnection
{
    private readonly NpgsqlDataSource _dataSource;
    private NpgsqlConnection? _inner;

    public LazyDbConnection(NpgsqlDataSource dataSource)
    {
        ArgumentNullException.ThrowIfNull(dataSource);
        _dataSource = dataSource;
    }

    /// <summary>
    /// Whether a pooled connection is currently held. Does not acquire one.
    /// </summary>
    public bool IsAcquired => _inner is not null;

    // Static data: reading it must not acquire a connection.
#pragma warning disable CS8765 // The base setter accepts null; a data-source-backed connection ignores changes
    public override string ConnectionString
    {
        get => _dataSource.ConnectionString;
        set => throw new NotSupportedException("The connection string is fixed by the data source.");
    }
#pragma warning restore CS8765

    public override string Database => Acquire().Database;

    public override string DataSource => Acquire().DataSource;

    public override string ServerVersion => Acquire().ServerVersion;

    // Any caller that inspects the state intends to use the connection, so report it as open.
    public override ConnectionState State => Acquire().State;

    public override void ChangeDatabase(string databaseName) => Acquire().ChangeDatabase(databaseName);

    public override void Open() => Acquire();

    public override Task OpenAsync(CancellationToken cancellationToken)
    {
        Acquire();
        return Task.CompletedTask;
    }

    public override void Close()
    {
        var inner = _inner;
        _inner = null;
        inner?.Close();
        inner?.Dispose();
    }

    protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel) =>
        Acquire().BeginTransaction(isolationLevel);

    protected override DbCommand CreateDbCommand() => Acquire().CreateCommand();

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Close();
        }

        base.Dispose(disposing);
    }

    private NpgsqlConnection Acquire() => _inner ??= _dataSource.OpenConnection();
}
