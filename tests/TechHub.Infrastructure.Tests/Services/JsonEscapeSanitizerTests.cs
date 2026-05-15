using FluentAssertions;
using TechHub.Infrastructure.Services;

namespace TechHub.Infrastructure.Tests.Services;

public class JsonEscapeSanitizerTests
{
    // ── Invalid escapes — backslash should be dropped ─────────────────────

    [Theory]
    [InlineData(@"\s", "s")]        // regex/shell pattern — the original production bug
    [InlineData(@"\.", ".")]        // previous production bug (domain/URL patterns)
    [InlineData(@"\-", "-")]        // dash in patterns
    [InlineData(@"\!", "!")]        // exclamation mark
    [InlineData(@"\p", "p")]        // another invalid escape
    public void SanitizeJsonEscapes_InvalidEscape_DropsBackslash(string input, string expected)
    {
        JsonEscapeSanitizer.SanitizeJsonEscapes(input).Should().Be(expected);
    }

    // ── Valid single-character escapes — must be preserved ────────────────

    [Theory]
    [InlineData(@"\n")]
    [InlineData(@"\r")]
    [InlineData(@"\t")]
    [InlineData(@"\b")]
    [InlineData(@"\f")]
    [InlineData(@"\/")]
    [InlineData(@"\\")]
    [InlineData("\\\"")]  // \"
    public void SanitizeJsonEscapes_ValidSingleCharEscape_Preserved(string input)
    {
        JsonEscapeSanitizer.SanitizeJsonEscapes(input).Should().Be(input);
    }

    // ── Valid \uXXXX escapes — must be preserved ──────────────────────────

    [Theory]
    [InlineData(@"\u0041")]   // 'A'
    [InlineData(@"\uD83D")]   // surrogate high
    [InlineData(@"\uFFFF")]
    [InlineData(@"\u00e9")]   // lowercase hex
    public void SanitizeJsonEscapes_ValidUnicodeEscape_Preserved(string input)
    {
        JsonEscapeSanitizer.SanitizeJsonEscapes(input).Should().Be(input);
    }

    // ── The key regression: \\s must NOT be corrupted ─────────────────────

    [Fact]
    public void SanitizeJsonEscapes_DoubleBackslashBeforeS_Preserved()
    {
        // \\s in inner JSON is valid JSON (represents the literal string \s, e.g. a regex pattern).
        // The previous regex incorrectly matched the second \+s and stripped the backslash,
        // turning valid \\s into invalid \s and causing a JsonException.
        const string Input = @"\\s";
        JsonEscapeSanitizer.SanitizeJsonEscapes(Input).Should().Be(@"\\s");
    }

    [Fact]
    public void SanitizeJsonEscapes_DoubleBackslashBeforeDot_Preserved()
    {
        // Same class of bug as \\s — the first \\ is valid, must not consume the second \
        const string Input = @"\\\.";
        JsonEscapeSanitizer.SanitizeJsonEscapes(Input).Should().Be(@"\\.");
    }

    [Fact]
    public void SanitizeJsonEscapes_MixedValidAndInvalidEscapes_OnlyFixesInvalid()
    {
        // Realistic inner JSON content field:
        //   "Use \\s+ pattern and foo\.bar"
        // \\s → valid (preserved), \. → invalid (dot kept, backslash dropped)
        const string Input = @"""Use \\s+ pattern and foo\.bar""";
        const string Expected = @"""Use \\s+ pattern and foo.bar""";

        JsonEscapeSanitizer.SanitizeJsonEscapes(Input).Should().Be(Expected);
    }

    [Fact]
    public void SanitizeJsonEscapes_NoEscapes_ReturnsUnchanged()
    {
        const string Input = """{"title":"Hello world","included":true}""";
        JsonEscapeSanitizer.SanitizeJsonEscapes(Input).Should().Be(Input);
    }

    [Fact]
    public void SanitizeJsonEscapes_EmptyString_ReturnsEmpty()
    {
        JsonEscapeSanitizer.SanitizeJsonEscapes(string.Empty).Should().BeEmpty();
    }
}
