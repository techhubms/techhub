using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using TechHub.Infrastructure.Services.ContentProcessing;

namespace TechHub.Infrastructure.Tests.Services;

/// <summary>
/// Unit tests for <see cref="TimelineEventParser"/>.
/// </summary>
public class TimelineEventParserTests
{
    private static readonly DateOnly _publicationDate = new(2026, 3, 14);

    private static JsonElement Array(string json) => JsonDocument.Parse(json).RootElement.Clone();

    private static string Event(
        string kind = "ga",
        string product = "GitHub Copilot",
        string? feature = "coding agent",
        string description = "Becomes generally available",
        string dateSource = "explicit",
        string? date = "2026-03-10",
        string? precision = "day",
        string? quote = "It is now GA.",
        string plans = "[]",
        string surfaces = "[]")
    {
        static string Q(string? value) => value is null ? "null" : JsonSerializer.Serialize(value);
        return $$"""
            {"kind":{{Q(kind)}},"product":{{Q(product)}},"feature":{{Q(feature)}},"plans":{{plans}},"surfaces":{{surfaces}},"description":{{Q(description)}},
             "date_source":{{Q(dateSource)}},"date":{{Q(date)}},"date_precision":{{Q(precision)}},"quote":{{Q(quote)}}}
            """;
    }

    private static IReadOnlyList<TechHub.Core.Models.ContentProcessing.TimelineEvent> Parse(params string[] events)
        => TimelineEventParser.Parse(Array("[" + string.Join(",", events) + "]"), _publicationDate, NullLogger.Instance, "https://example.com/a");

    [Fact]
    public void Parse_PlansAndSurfaces_AreKeptAsReturnedWithoutDuplicatesOrBlanks()
    {
        var result = Parse(Event(plans: """["Business", " Enterprise ", "business", "", "  "]""", surfaces: """["VS Code", "JetBrains IDEs"]"""));

        var e = result.Should().ContainSingle().Subject;
        e.Plans.Should().Equal("Business", "Enterprise");
        e.Surfaces.Should().Equal("VS Code", "JetBrains IDEs");
    }

    [Fact]
    public void Parse_WithoutPlansAndSurfaces_GivesEmptyLists()
    {
        var json = """[{"kind":"ga","product":"X","feature":null,"description":"d","date_source":"publication","date":null,"date_precision":null,"quote":null}]""";

        var result = TimelineEventParser.Parse(Array(json), _publicationDate, NullLogger.Instance, "https://example.com/a");

        var e = result.Should().ContainSingle().Subject;
        e.Plans.Should().BeEmpty();
        e.Surfaces.Should().BeEmpty();
    }

    [Fact]
    public void Parse_PlansAndSurfaces_AreLimitedInNumberAndLength()
    {
        var many = string.Join(",", Enumerable.Range(1, 12).Select(i => $"\"Plan {i}\""));
        var tooLong = new string('x', 101);

        var result = Parse(Event(plans: $"[{many}]", surfaces: $"[\"{tooLong}\", \"Xcode\", 7, null]"));

        var e = result.Should().ContainSingle().Subject;
        e.Plans.Should().HaveCount(8);
        e.Surfaces.Should().Equal("Xcode");
    }

    [Fact]
    public void Parse_SameKindProductAndFeatureForDifferentSurfaces_KeepsBothEvents()
    {
        var result = Parse(
            Event(surfaces: """["Visual Studio Code"]""", date: "2026-03-10"),
            Event(surfaces: """["JetBrains IDEs"]""", date: "2026-04-20"),
            Event(surfaces: """["jetbrains ides"]""", date: "2026-05-01"));

        result.Should().HaveCount(2);
        result.Select(e => e.Surfaces.Single()).Should().Equal("Visual Studio Code", "JetBrains IDEs");
    }

    [Fact]
    public void Parse_ExplicitDay_KeepsAllFields()
    {
        var result = Parse(Event());

        var e = result.Should().ContainSingle().Subject;
        e.Kind.Should().Be("ga");
        e.Product.Should().Be("GitHub Copilot");
        e.Feature.Should().Be("coding agent");
        e.Description.Should().Be("Becomes generally available");
        e.Date.Should().Be(new DateOnly(2026, 3, 10));
        e.DatePrecision.Should().Be("day");
        e.DateSource.Should().Be("explicit");
        e.Quote.Should().Be("It is now GA.");
    }

    [Fact]
    public void Parse_PublicationSource_UsesPublicationDateAndIgnoresModelDate()
    {
        var result = Parse(Event(dateSource: "publication", date: "1999-01-01", precision: "month"));

        var e = result.Should().ContainSingle().Subject;
        e.Date.Should().Be(_publicationDate);
        e.DatePrecision.Should().Be("day");
        e.DateSource.Should().Be("publication");
    }

    [Fact]
    public void Parse_MonthPrecision_NormalizesToFirstDayOfMonth()
    {
        var result = Parse(Event(date: "2026-05-17", precision: "month"));

        result.Should().ContainSingle().Which.Date.Should().Be(new DateOnly(2026, 5, 1));
    }

    [Theory]
    [InlineData("2026-02-20", "2026-01-01")]
    [InlineData("2026-04-01", "2026-04-01")]
    [InlineData("2026-08-31", "2026-07-01")]
    [InlineData("2026-12-15", "2026-10-01")]
    public void Parse_QuarterPrecision_NormalizesToFirstDayOfQuarter(string date, string expected)
    {
        var result = Parse(Event(date: date, precision: "quarter"));

        result.Should().ContainSingle().Which.Date.Should().Be(DateOnly.Parse(expected, System.Globalization.CultureInfo.InvariantCulture));
    }

    [Theory]
    [InlineData("not a date")]
    [InlineData("2026-13-40")]
    [InlineData("")]
    [InlineData(null)]
    public void Parse_ExplicitWithUnparseableDate_SkipsTheEvent(string? date)
    {
        Parse(Event(date: date)).Should().BeEmpty();
    }

    [Fact]
    public void Parse_UnknownKind_SkipsTheEvent()
    {
        Parse(Event(kind: "rumor")).Should().BeEmpty();
    }

    [Fact]
    public void Parse_MissingProductOrDescription_SkipsTheEvent()
    {
        Parse(Event(product: " "), Event(description: "")).Should().BeEmpty();
    }

    [Fact]
    public void Parse_BlankFeatureAndQuote_BecomeNull()
    {
        var result = Parse(Event(feature: "  ", quote: " "));

        var e = result.Should().ContainSingle().Subject;
        e.Feature.Should().BeNull();
        e.Quote.Should().BeNull();
    }

    [Fact]
    public void Parse_NullPrecisionForExplicitDate_DefaultsToDay()
    {
        var result = Parse(Event(precision: null));

        result.Should().ContainSingle().Which.DatePrecision.Should().Be("day");
    }

    [Fact]
    public void Parse_UnknownPrecision_SkipsTheEvent()
    {
        Parse(Event(precision: "decade")).Should().BeEmpty();
    }

    [Fact]
    public void Parse_DuplicateKindProductFeature_KeepsFirst()
    {
        var result = Parse(Event(description: "first"), Event(description: "second", date: "2026-03-11"));

        result.Should().ContainSingle().Which.Description.Should().Be("first");
    }

    [Fact]
    public void Parse_MoreThanTenEvents_KeepsFirstTen()
    {
        var events = Enumerable.Range(1, 12).Select(i => Event(product: $"Product {i}")).ToArray();

        Parse(events).Should().HaveCount(10);
    }

    [Fact]
    public void Parse_NotAnArray_ReturnsEmpty()
    {
        var result = TimelineEventParser.Parse(Array("{}"), _publicationDate, NullLogger.Instance, "https://example.com/a");

        result.Should().BeEmpty();
    }

    [Fact]
    public void GetBrusselsDate_UsesEuropeBrusselsNotUtc()
    {
        // 23:30 UTC on 14 March is already 15 March in Brussels (CET).
        var published = new DateTimeOffset(2026, 3, 14, 23, 30, 0, TimeSpan.Zero);

        TimelineEventParser.GetBrusselsDate(published).Should().Be(new DateOnly(2026, 3, 15));
    }
}
