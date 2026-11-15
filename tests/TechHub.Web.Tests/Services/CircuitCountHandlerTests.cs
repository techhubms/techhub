using FluentAssertions;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Services;

public class CircuitCountHandlerTests
{
    private readonly CircuitCountHandler _sut = new();

    [Fact]
    public void Counts_FollowTheCircuitLifecycle()
    {
        // Act & Assert — a circuit is open from the start but only connected once the client attaches
        _sut.CircuitOpened("a");
        _sut.CircuitOpened("b");
        _sut.ConnectionUp("a");
        _sut.Open.Should().Be(2);
        _sut.Connected.Should().Be(1);

        // A dropped connection leaves the circuit open (retained) until it is closed
        _sut.ConnectionDown("a");
        _sut.Open.Should().Be(2);
        _sut.Connected.Should().Be(0);

        _sut.CircuitClosed("a");
        _sut.Open.Should().Be(1);
    }

    [Fact]
    public void CircuitClosed_WhileConnected_DoesNotLeaveAConnectedCircuitBehind()
    {
        // Arrange — a graceful close can happen without a connection-down callback
        _sut.CircuitOpened("a");
        _sut.ConnectionUp("a");

        // Act
        _sut.CircuitClosed("a");

        // Assert
        _sut.Open.Should().Be(0);
        _sut.Connected.Should().Be(0);
    }

    [Fact]
    public void RepeatedCallbacks_DoNotInflateTheCounts()
    {
        // Act
        _sut.CircuitOpened("a");
        _sut.CircuitOpened("a");
        _sut.ConnectionUp("a");
        _sut.ConnectionUp("a");

        // Assert
        _sut.Open.Should().Be(1);
        _sut.Connected.Should().Be(1);
    }
}
