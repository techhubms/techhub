using FluentAssertions;
using TechHub.Core.Validation;

namespace TechHub.Core.Tests.Validation;

public class RouteParameterValidatorTests
{
    [Theory]
    [InlineData("ai")]
    [InlineData("all")]
    [InlineData("github-copilot")]
    [InlineData("dotnet")]
    [InlineData("devops")]
    [InlineData("azure")]
    [InlineData("ml")]
    [InlineData("security")]
    [InlineData("news")]
    [InlineData("blogs")]
    [InlineData("videos")]
    [InlineData("community")]
    [InlineData("levels-of-enlightenment")]
    [InlineData("AI")]
    [InlineData("GitHub-Copilot")]
    [InlineData("DotNet")]
    [InlineData("Videos")]
    public void IsValidNameSegment_WithValidNames_ReturnsTrue(string name)
    {
        RouteParameterValidator.IsValidNameSegment(name).Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("#skiptohere")]
    [InlineData("%23skiptohere")]
    [InlineData("pp.php")]
    [InlineData("admin/passwd")]
    [InlineData("../etc")]
    [InlineData("has space")]
    [InlineData("has_underscore")]
    [InlineData("has.dot")]
    [InlineData("-starts-with-hyphen")]
    [InlineData("123numeric")]
    public void IsValidNameSegment_WithInvalidNames_ReturnsFalse(string? name)
    {
        RouteParameterValidator.IsValidNameSegment(name).Should().BeFalse();
    }

    [Theory]
    [InlineData("how-copilot-helps")]
    [InlineData("weekly-ai-and-tech-news-roundup-2026-03-16")]
    [InlineData("github-copilot-video-series")]
    [InlineData("encrypting-properties-with-systemtextjson")]
    [InlineData("fts-test")]
    [InlineData("123-numeric-start")]
    [InlineData("HAS-UPPER")]
    [InlineData("Mixed-Case-Slug")]
    public void IsValidSlug_WithValidSlugs_ReturnsTrue(string slug)
    {
        RouteParameterValidator.IsValidSlug(slug).Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("file.php")]
    [InlineData("../../etc/passwd")]
    [InlineData("script<alert>")]
    [InlineData("has space")]
    [InlineData("#anchor")]
    [InlineData("-starts-with-hyphen")]
    public void IsValidSlug_WithInvalidSlugs_ReturnsFalse(string? slug)
    {
        RouteParameterValidator.IsValidSlug(slug).Should().BeFalse();
    }

    [Fact]
    public void IsValidNameSegment_WithExcessiveLength_ReturnsFalse()
    {
        var longName = new string('a', 201);
        RouteParameterValidator.IsValidNameSegment(longName).Should().BeFalse();
    }

    [Fact]
    public void IsValidSlug_WithExcessiveLength_ReturnsFalse()
    {
        var longSlug = new string('a', 201);
        RouteParameterValidator.IsValidSlug(longSlug).Should().BeFalse();
    }
}
