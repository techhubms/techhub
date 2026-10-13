using System.Collections.Concurrent;
using System.Net;
using System.Text;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace TechHub.Api.Tests.Endpoints;

public sealed class RejectedRequestLoggingTests : IClassFixture<TechHubIntegrationTestApiFactory>, IDisposable
{
    private const string LoggerCategory = "TechHub.Api.Endpoints.RejectedRequestLogging";

    private readonly HttpClient _client;
    private readonly CapturingLoggerProvider _logs = new(LoggerCategory);

    public RejectedRequestLoggingTests(TechHubIntegrationTestApiFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _client = factory.CreateClient();
        factory.Services.GetRequiredService<ILoggerFactory>().AddProvider(_logs);
    }

    public void Dispose() => _logs.Dispose();

    [Fact]
    public async Task HandlerReturnedBadRequest_LogsWarningWithMethodPathAndReason()
    {
        // Act
        var response = await _client.GetAsync("/api/newsletter/confirm", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var entry = _logs.Entries.Should().ContainSingle().Subject;
        entry.Level.Should().Be(LogLevel.Warning);
        entry.Message.Should().Contain("GET")
            .And.Contain("/api/newsletter/confirm")
            .And.Contain("Email and token are required.");
    }

    [Fact]
    public async Task HandlerReturnedBadRequest_DoesNotLogQueryStringValues()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/newsletter/confirm?email=leaked-user@example.com&token=leaked-token-value",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var entry = _logs.Entries.Should().ContainSingle().Subject;
        entry.Message.Should().Contain("/api/newsletter/confirm")
            .And.NotContain("leaked-user@example.com")
            .And.NotContain("leaked-token-value");
    }

    [Fact]
    public async Task HandlerReturnedBadRequest_DoesNotLogEchoedQueryValues()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/sections/all/collections/all/items?from=secret-from-value",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var entry = _logs.Entries.Should().ContainSingle().Subject;
        entry.Message.Should().NotContain("secret-from-value");
    }

    [Fact]
    public async Task SuccessfulRequest_IsNotLogged()
    {
        // Act
        var response = await _client.GetAsync("/api/sections", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        _logs.Entries.Should().BeEmpty();
    }

    [Fact]
    public async Task NotFoundResponse_IsNotLogged()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/invalid/collections/all/items", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        _logs.Entries.Should().BeEmpty();
    }

    [Fact]
    public async Task FrameworkBindingFailure_ReturnsBadRequestWithoutHandlerLog()
    {
        // Arrange
        using var content = new StringContent("{ not valid json", Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/newsletter/subscribe", content, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        _logs.Entries.Should().BeEmpty("the filter only observes responses returned by endpoint handlers");
    }

    private sealed record LogEntry(LogLevel Level, string Message);

    private sealed class CapturingLoggerProvider : ILoggerProvider
    {
        private readonly string _category;
        private volatile bool _disposed;

        public CapturingLoggerProvider(string category) => _category = category;

        public ConcurrentQueue<LogEntry> Entries { get; } = new();

        public ILogger CreateLogger(string categoryName) =>
            categoryName == _category ? new CapturingLogger(this) : NullLogger.Instance;

        public void Dispose() => _disposed = true;

        private sealed class CapturingLogger : ILogger
        {
            private readonly CapturingLoggerProvider _owner;

            public CapturingLogger(CapturingLoggerProvider owner) => _owner = owner;

            public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

            public bool IsEnabled(LogLevel logLevel) => !_owner._disposed;

            public void Log<TState>(
                LogLevel logLevel,
                EventId eventId,
                TState state,
                Exception? exception,
                Func<TState, Exception?, string> formatter)
            {
                if (!_owner._disposed)
                {
                    _owner.Entries.Enqueue(new LogEntry(logLevel, formatter(state, exception)));
                }
            }
        }
    }
}
