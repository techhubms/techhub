using FluentAssertions;
using Microsoft.Extensions.Logging;
using TechHub.Core.Logging;

namespace TechHub.Core.Tests.Logging;

public class LoggerSanitizedExtensionsTests
{
    private sealed class SpyLogger : ILogger
    {
        public string? LastFormattedMessage { get; private set; }
        public Exception? LastException { get; private set; }
        public LogLevel? LastLogLevel { get; private set; }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            LastLogLevel = logLevel;
            LastException = exception;
            LastFormattedMessage = formatter(state, exception);
        }
    }

    [Fact]
    public void LogDebugSanitized_SanitizesStringArgs()
    {
        // Arrange
        var logger = new SpyLogger();

        // Act
        logger.LogDebugSanitized("Section: {Name}", "ai\ninjection");

        // Assert
        logger.LastLogLevel.Should().Be(LogLevel.Debug);
        logger.LastFormattedMessage.Should().NotContain("\n");
        logger.LastFormattedMessage.Should().Contain("aiinjection");
    }

    [Fact]
    public void LogInformationSanitized_SanitizesStringArgs()
    {
        // Arrange
        var logger = new SpyLogger();

        // Act
        logger.LogInformationSanitized("Redirect: {Host}", "host\r\nFAKE LOG");

        // Assert
        logger.LastLogLevel.Should().Be(LogLevel.Information);
        logger.LastFormattedMessage.Should().NotContain("\r").And.NotContain("\n");
        logger.LastFormattedMessage.Should().Contain("hostFAKE LOG");
    }

    [Fact]
    public void LogWarningSanitized_SanitizesStringArgs()
    {
        // Arrange
        var logger = new SpyLogger();

        // Act
        logger.LogWarningSanitized("Not found: {Section}", "missing\nsection");

        // Assert
        logger.LastLogLevel.Should().Be(LogLevel.Warning);
        logger.LastFormattedMessage.Should().NotContain("\n");
    }

    [Fact]
    public void LogErrorSanitized_WithException_SanitizesStringArgs()
    {
        // Arrange
        var logger = new SpyLogger();
        var exception = new InvalidOperationException("test");

        // Act
        logger.LogErrorSanitized(exception, "Failed: {Path}", "/api\ninjected");

        // Assert
        logger.LastLogLevel.Should().Be(LogLevel.Error);
        logger.LastException.Should().BeSameAs(exception);
        logger.LastFormattedMessage.Should().NotContain("\n");
        logger.LastFormattedMessage.Should().Contain("/apiinjected");
    }

    [Fact]
    public void LogErrorSanitized_WithoutException_SanitizesStringArgs()
    {
        // Arrange
        var logger = new SpyLogger();

        // Act
        logger.LogErrorSanitized("Error: {Value}", "bad\rvalue");

        // Assert
        logger.LastLogLevel.Should().Be(LogLevel.Error);
        logger.LastException.Should().BeNull();
        logger.LastFormattedMessage.Should().NotContain("\r");
    }

    [Fact]
    public void LogDebugSanitized_PreservesNonStringArgs()
    {
        // Arrange
        var logger = new SpyLogger();

        // Act
        logger.LogDebugSanitized("Fetched {Count} items for {Section}", 42, "ai");

        // Assert
        logger.LastFormattedMessage.Should().Be("Fetched 42 items for ai");
    }

    [Fact]
    public void LogDebugSanitized_WithCleanStrings_PassesThroughUnchanged()
    {
        // Arrange
        var logger = new SpyLogger();

        // Act
        logger.LogDebugSanitized("Section: {Name}", "ai-section");

        // Assert
        logger.LastFormattedMessage.Should().Be("Section: ai-section");
    }

    [Fact]
    public void LogDebugSanitized_SanitizesMultipleStringArgs()
    {
        // Arrange
        var logger = new SpyLogger();

        // Act
        logger.LogDebugSanitized("Fetching {Section}/{Collection}", "sec\ntion", "col\nlection");

        // Assert
        logger.LastFormattedMessage.Should().NotContain("\n");
        logger.LastFormattedMessage.Should().Be("Fetching section/collection");
    }

    [Fact]
    public void LogDebugSanitized_WithNullArg_PassesNullThrough()
    {
        // Arrange
        var logger = new SpyLogger();

        // Act
        logger.LogDebugSanitized("Value: {Value}", (object?)null);

        // Assert
        logger.LastFormattedMessage.Should().Be("Value: (null)");
    }
}
