using FluentAssertions;
using TechHub.Core.Logging;

namespace TechHub.Core.Tests.Logging;

public class LogSanitizerTests
{
    [Fact]
    public void Sanitize_NullValue_ReturnsEmpty()
    {
        // Act
        var result = LogSanitizer.Sanitize(null);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void Sanitize_EmptyString_ReturnsEmpty()
    {
        // Act
        var result = LogSanitizer.Sanitize(string.Empty);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void Sanitize_CleanValue_ReturnsUnchanged()
    {
        // Arrange
        var input = "/api/sections/ai/collections/blogs";

        // Act
        var result = LogSanitizer.Sanitize(input);

        // Assert
        result.Should().Be(input);
    }

    [Fact]
    public void Sanitize_NewlineCharacters_RemovesThem()
    {
        // Arrange
        var input = "value\ninjected log entry";

        // Act
        var result = LogSanitizer.Sanitize(input);

        // Assert
        result.Should().Be("valueinjected log entry");
    }

    [Fact]
    public void Sanitize_CarriageReturnCharacters_RemovesThem()
    {
        // Arrange
        var input = "value\rinjected log entry";

        // Act
        var result = LogSanitizer.Sanitize(input);

        // Assert
        result.Should().Be("valueinjected log entry");
    }

    [Fact]
    public void Sanitize_CrLfSequence_RemovesBoth()
    {
        // Arrange
        var input = "value\r\nINFO: fake log entry";

        // Act
        var result = LogSanitizer.Sanitize(input);

        // Assert
        result.Should().Be("valueINFO: fake log entry");
    }

    [Fact]
    public void Sanitize_MultipleNewlines_RemovesAll()
    {
        // Arrange
        var input = "line1\nline2\r\nline3\nline4";

        // Act
        var result = LogSanitizer.Sanitize(input);

        // Assert
        result.Should().Be("line1line2line3line4");
    }

    [Theory]
    [InlineData("/path?q=test", "/path?q=test")]
    [InlineData("host.example.com", "host.example.com")]
    [InlineData("section/collection/slug", "section/collection/slug")]
    public void Sanitize_TypicalLogValues_ReturnsUnchanged(string input, string expected)
    {
        // Act
        var result = LogSanitizer.Sanitize(input);

        // Assert
        result.Should().Be(expected);
    }
}
