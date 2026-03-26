using FluentAssertions;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Services;

/// <summary>
/// Tests for DateHelper.FormatTimestamp which formats DateTimeOffset values
/// in Europe/Brussels timezone.
/// </summary>
public class DateHelperFormatTimestampTests
{
    [Fact]
    public void FormatTimestamp_ConvertsToEuropeBrusselsTimezone()
    {
        // Arrange - 15:00 UTC in summer = 17:00 Brussels (CEST = UTC+2)
        var timestamp = new DateTimeOffset(2026, 7, 15, 15, 30, 45, TimeSpan.Zero);

        // Act
        var result = DateHelper.FormatTimestamp(timestamp);

        // Assert
        result.Should().Be("2026-07-15 17:30:45");
    }

    [Fact]
    public void FormatTimestamp_ConvertsToEuropeBrusselsTimezone_Winter()
    {
        // Arrange - 15:00 UTC in winter = 16:00 Brussels (CET = UTC+1)
        var timestamp = new DateTimeOffset(2026, 1, 15, 15, 30, 45, TimeSpan.Zero);

        // Act
        var result = DateHelper.FormatTimestamp(timestamp);

        // Assert
        result.Should().Be("2026-01-15 16:30:45");
    }

    [Fact]
    public void FormatTimestamp_UsesExpectedFormat()
    {
        // Arrange
        var timestamp = new DateTimeOffset(2026, 3, 25, 10, 5, 3, TimeSpan.Zero);

        // Act
        var result = DateHelper.FormatTimestamp(timestamp);

        // Assert - format is "yyyy-MM-dd HH:mm:ss"
        result.Should().MatchRegex(@"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}$");
    }
}
