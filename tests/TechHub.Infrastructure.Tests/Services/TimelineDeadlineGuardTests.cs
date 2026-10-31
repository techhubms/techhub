using FluentAssertions;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Infrastructure.Services.ContentProcessing;

namespace TechHub.Infrastructure.Tests.Services;

/// <summary>
/// Unit tests for <see cref="TimelineDeadlineGuard"/>: a future date next to a deadline phrase that no event covers.
/// </summary>
public class TimelineDeadlineGuardTests
{
    private static readonly DateOnly _published = new(2026, 10, 21);

    private static TimelineEvent Event(string date, string precision = "day") => new()
    {
        Kind = "retirement",
        Product = "Product",
        Description = "Is retired",
        Date = DateOnly.Parse(date, System.Globalization.CultureInfo.InvariantCulture),
        DatePrecision = precision,
        DateSource = TimelineEvent.DateSourceExplicit
    };

    [Theory]
    [InlineData("Microsoft Foundry is retiring workflows on **December 1, 2026**. After that date the designer is not supported.")]
    [InlineData("Support for Java 11 ends on 1 September 2027, so plan your upgrade. This is the end of support for the runtime.")]
    [InlineData("The legacy API will be deprecated on 2027-01-15 and removed later.")]
    [InlineData("The old endpoint is shutting down on November 30th, 2026.")]
    public void MissesDeadline_FutureDateNextToDeadlinePhraseWithoutEvent_ReturnsTrue(string text)
    {
        TimelineDeadlineGuard.MissesDeadline(text, [], _published).Should().BeTrue();
    }

    [Fact]
    public void MissesDeadline_EventHasTheDate_ReturnsFalse()
    {
        var text = "Microsoft Foundry is retiring workflows on December 1, 2026.";

        TimelineDeadlineGuard.MissesDeadline(text, [Event("2026-12-01")], _published).Should().BeFalse();
    }

    [Fact]
    public void MissesDeadline_MonthPrecisionEventInTheSameMonth_ReturnsFalse()
    {
        var text = "Support ends on December 1, 2026.";

        TimelineDeadlineGuard.MissesDeadline(text, [Event("2026-12-01", "month")], _published).Should().BeFalse();
    }

    [Theory]
    [InlineData("The conference takes place on November 16, 2026 in San Francisco.")]
    [InlineData("Version 2 was announced on March 3, 2026 and is deprecated today.")]
    [InlineData("It is generally available on December 1, 2026.")]
    public void MissesDeadline_NoDeadlinePhraseOrDateNotInTheFuture_ReturnsFalse(string text)
    {
        TimelineDeadlineGuard.MissesDeadline(text, [], _published).Should().BeFalse();
    }

    [Fact]
    public void MissesDeadline_DeadlinePhraseFarFromTheDate_ReturnsFalse()
    {
        var text = "This feature is deprecated." + new string('x', 400) + " Join us on December 1, 2026.";

        TimelineDeadlineGuard.MissesDeadline(text, [], _published).Should().BeFalse();
    }

    [Fact]
    public void MissesDeadline_ImpossibleDate_ReturnsFalse()
    {
        TimelineDeadlineGuard.MissesDeadline("Retired on February 31, 2027.", [], _published).Should().BeFalse();
    }

    [Fact]
    public void MissesDeadline_DateMoreThanFiveYearsAhead_ReturnsFalse()
    {
        TimelineDeadlineGuard.MissesDeadline("Support ends on 1 January 2040.", [], _published).Should().BeFalse();
    }
}
