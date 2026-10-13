using FluentAssertions;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Services;

/// <summary>
/// Tests for DateHelper.FormatTimestampIso which formats timestamps as ISO 8601
/// with the Europe/Brussels offset.
/// </summary>
public class DateHelperFormatTimestampIsoTests
{
    [Theory]
    [InlineData(2026, 7, 15, 15, 30, 45, "2026-07-15T17:30:45+02:00")]
    [InlineData(2026, 1, 15, 15, 30, 45, "2026-01-15T16:30:45+01:00")]
    public void FormatTimestampIso_ConvertsToBrusselsWithOffset(int year, int month, int day, int hour, int minute, int second, string expected)
    {
        // Arrange
        var timestamp = new DateTimeOffset(year, month, day, hour, minute, second, TimeSpan.Zero);

        // Act
        var result = DateHelper.FormatTimestampIso(timestamp);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void FormatTimestampIso_EpochOverload_MatchesDateTimeOffsetOverload()
    {
        // Arrange
        var timestamp = new DateTimeOffset(2026, 9, 24, 8, 0, 0, TimeSpan.Zero);

        // Act
        var result = DateHelper.FormatTimestampIso(timestamp.ToUnixTimeSeconds());

        // Assert
        result.Should().Be(DateHelper.FormatTimestampIso(timestamp)).And.Be("2026-09-24T10:00:00+02:00");
    }
}
