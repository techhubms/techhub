using FluentAssertions;
using TechHub.Core.Logging;

namespace TechHub.Core.Tests.Logging;

public class InputSanitizerTests
{
    [Fact]
    public void Sanitize_NullValue_ReturnsEmpty()
    {
        // Act
        var result = InputSanitizer.Sanitize(null);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void Sanitize_EmptyString_ReturnsEmpty()
    {
        // Act
        var result = InputSanitizer.Sanitize(string.Empty);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void Sanitize_CleanValue_ReturnsUnchanged()
    {
        // Arrange
        var input = "/api/sections/ai/collections/blogs";

        // Act
        var result = InputSanitizer.Sanitize(input);

        // Assert
        result.Should().Be(input);
    }

    [Fact]
    public void Sanitize_NewlineCharacters_RemovesThem()
    {
        // Arrange
        var input = "value\ninjected log entry";

        // Act
        var result = InputSanitizer.Sanitize(input);

        // Assert
        result.Should().Be("valueinjected log entry");
    }

    [Fact]
    public void Sanitize_CarriageReturnCharacters_RemovesThem()
    {
        // Arrange
        var input = "value\rinjected log entry";

        // Act
        var result = InputSanitizer.Sanitize(input);

        // Assert
        result.Should().Be("valueinjected log entry");
    }

    [Fact]
    public void Sanitize_CrLfSequence_RemovesBoth()
    {
        // Arrange
        var input = "value\r\nINFO: fake log entry";

        // Act
        var result = InputSanitizer.Sanitize(input);

        // Assert
        result.Should().Be("valueINFO: fake log entry");
    }

    [Fact]
    public void Sanitize_MultipleNewlines_RemovesAll()
    {
        // Arrange
        var input = "line1\nline2\r\nline3\nline4";

        // Act
        var result = InputSanitizer.Sanitize(input);

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
        var result = InputSanitizer.Sanitize(input);

        // Assert
        result.Should().Be(expected);
    }

    // ─── MaskEmail ───────────────────────────────────────────────────────────

    [Fact]
    public void MaskEmail_NullValue_ReturnsEmpty()
    {
        InputSanitizer.MaskEmail(null).Should().BeEmpty();
    }

    [Fact]
    public void MaskEmail_EmptyString_ReturnsEmpty()
    {
        InputSanitizer.MaskEmail(string.Empty).Should().BeEmpty();
    }

    [Theory]
    [InlineData("user@example.com", "u***@example.com")]
    [InlineData("reinier@xebia.com", "r***@xebia.com")]
    [InlineData("a@b.com", "a***@b.com")]
    [InlineData("newsletter@mail.hub.ms", "n***@mail.hub.ms")]
    public void MaskEmail_ValidEmail_MasksLocalPart(string input, string expected)
    {
        InputSanitizer.MaskEmail(input).Should().Be(expected);
    }

    [Fact]
    public void MaskEmail_NoAtSign_ReturnsSanitizedValue()
    {
        // Not a recognisable email — falls back to Sanitize behaviour (no crash).
        InputSanitizer.MaskEmail("notanemail").Should().Be("notanemail");
    }

    [Fact]
    public void MaskEmail_EmailWithNewline_MasksAndSanitizes()
    {
        // Log-forging attempt embedded in an otherwise valid email.
        InputSanitizer.MaskEmail("u\nser@example.com").Should().Be("u***@example.com");
    }
}
