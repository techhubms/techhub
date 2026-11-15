using System.Collections.Concurrent;
using Microsoft.AspNetCore.Components.Server.Circuits;

namespace TechHub.Web.Services;

/// <summary>
/// Counts open and connected Blazor circuits. Together with the heap metrics this shows whether memory
/// growth follows the number of circuits (open minus connected are the retained disconnected ones).
/// Circuits are tracked by id, so a circuit that closes without a connection-down callback cannot skew the counts.
/// </summary>
public sealed class CircuitCountHandler : CircuitHandler
{
    private readonly ConcurrentDictionary<string, byte> _open = new(StringComparer.Ordinal);
    private readonly ConcurrentDictionary<string, byte> _connected = new(StringComparer.Ordinal);

    /// <summary>Circuits that exist on the server, connected or not.</summary>
    public long Open => _open.Count;

    /// <summary>Circuits that currently have a client connection.</summary>
    public long Connected => _connected.Count;

    public override Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(circuit);
        CircuitOpened(circuit.Id);
        return Task.CompletedTask;
    }

    public override Task OnCircuitClosedAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(circuit);
        CircuitClosed(circuit.Id);
        return Task.CompletedTask;
    }

    public override Task OnConnectionUpAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(circuit);
        ConnectionUp(circuit.Id);
        return Task.CompletedTask;
    }

    public override Task OnConnectionDownAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(circuit);
        ConnectionDown(circuit.Id);
        return Task.CompletedTask;
    }

    internal void CircuitOpened(string circuitId) => _open[circuitId] = 0;

    internal void CircuitClosed(string circuitId)
    {
        _open.TryRemove(circuitId, out _);
        _connected.TryRemove(circuitId, out _);
    }

    internal void ConnectionUp(string circuitId) => _connected[circuitId] = 0;

    internal void ConnectionDown(string circuitId) => _connected.TryRemove(circuitId, out _);
}
