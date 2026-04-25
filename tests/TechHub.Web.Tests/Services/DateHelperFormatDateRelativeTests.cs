using FluentAssertions;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Services;

/// <summary>
/// Tests for DateHelper.FormatDateRelative which formats Unix epoch timestamps
/// as relative date strings using the Europe/Brussels timezone.
/// </summary>
public class DateHelperFormatDateRelativeTests
{
    [Fact]
    public void FormatDateRelative_Today_ReturnsToday()
    {
        // Arrange - exactly the current day in Brussels (we just need "Today" to round-trip,
        // so use DateTimeOffset.UtcNow which DateHelper also uses for "today")
        var epoch = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        // Act
        var result = DateHelper.FormatDateRelative(epoch);

        // Assert
        result.Should().Be("Today");
    }

    [Fact]
    public void FormatDateRelative_Yesterday_ReturnsYesterday()
    {
        // Arrange
        var epoch = DateTimeOffset.UtcNow.AddDays(-1).ToUnixTimeSeconds();

        // Act
        var result = DateHelper.FormatDateRelative(epoch);

        // Assert
        result.Should().Be("Yesterday");
    }

    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(6)]
    public void FormatDateRelative_DaysAgo_ReturnsDaysAgoString(int daysAgo)
    {
        // Arrange
        var epoch = DateTimeOffset.UtcNow.AddDays(-daysAgo).ToUnixTimeSeconds();

        // Act
        var result = DateHelper.FormatDateRelative(epoch);

        // Assert
        result.Should().Be($"{daysAgo} days ago");
    }

    [Fact]
    public void FormatDateRelative_Tomorrow_ReturnsTomorrow()
    {
        // Arrange
        var epoch = DateTimeOffset.UtcNow.AddDays(1).ToUnixTimeSeconds();

        // Act
        var result = DateHelper.FormatDateRelative(epoch);

        // Assert
        result.Should().Be("Tomorrow");
    }

    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(6)]
    public void FormatDateRelative_FutureDays_ReturnsInXDays(int futureDays)
    {
        // Arrange
        var epoch = DateTimeOffset.UtcNow.AddDays(futureDays).ToUnixTimeSeconds();

        // Act
        var result = DateHelper.FormatDateRelative(epoch);

        // Assert
        result.Should().Be($"In {futureDays} days");
    }

    [Theory]
    [InlineData(7, "In 1 week")]
    [InlineData(8, "In 1 week")]
    [InlineData(13, "In 1 week")]
    [InlineData(14, "In 2 weeks")]
    [InlineData(21, "In 3 weeks")]
    [InlineData(28, "In 4 weeks")]
    public void FormatDateRelative_FutureWeeks_ReturnsCorrectPluralisation(int futureDays, string expected)
    {
        // Arrange
        var epoch = DateTimeOffset.UtcNow.AddDays(futureDays).ToUnixTimeSeconds();

        // Act
        var result = DateHelper.FormatDateRelative(epoch);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void FormatDateRelative_FarFuture_ReturnsFormattedDate()
    {
        // Arrange — more than 30 days in the future should return a formatted date string
        var futureDate = new DateTimeOffset(2030, 12, 25, 12, 0, 0, TimeSpan.Zero);
        var epoch = futureDate.ToUnixTimeSeconds();

        // Act
        var result = DateHelper.FormatDateRelative(epoch);

        // Assert — formatted as "MMM d, yyyy" (invariant culture)
        result.Should().Be("Dec 25, 2030");
    }
}
