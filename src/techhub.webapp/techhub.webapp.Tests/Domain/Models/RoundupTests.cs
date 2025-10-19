using techhub.webapp.ApiService.Domain.Models;

namespace techhub.webapp.Tests.Domain.Models;

public class RoundupTests
{
    [Fact]
    public void Validate_WithValidData_SetsCollectionType()
    {
        // Arrange
        var roundup = new Roundup
        {
            Title = "Weekly Roundup",
            Description = "Weekly content summary",
            Author = "Tech Hub Team",
            PublishedDate = DateTimeOffset.UtcNow,
            Categories = ["AI"],
            Tags = ["roundup", "weekly", "summary"],
            TagsNormalized = ["roundup", "weekly", "summary"],
            Permalink = "/roundup.html",
            ViewingMode = "internal",
            RoundupPeriod = "Week of Jan 1-7, 2025"
        };

        // Act & Assert
        roundup.Validate(); // Should not throw
        Assert.Equal("roundups", roundup.CollectionType);
    }

    [Fact]
    public void Validate_WithEmptyRoundupPeriod_ThrowsException()
    {
        // Arrange
        var roundup = new Roundup
        {
            Title = "Weekly Roundup",
            Description = "Weekly content summary",
            Author = "Tech Hub Team",
            PublishedDate = DateTimeOffset.UtcNow,
            Categories = ["AI"],
            Tags = ["roundup", "weekly", "summary"],
            TagsNormalized = ["roundup", "weekly", "summary"],
            Permalink = "/roundup.html",
            ViewingMode = "internal",
            RoundupPeriod = ""
        };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => roundup.Validate());
        Assert.Contains("RoundupPeriod cannot be empty", ex.Message);
    }

    [Theory]
    [InlineData("Week of Jan 1-7, 2025")]
    [InlineData("Jan 1-7, 2025")]
    [InlineData("Q1 2025")]
    public void Validate_WithVariousRoundupPeriodFormats_Succeeds(string period)
    {
        // Arrange
        var roundup = new Roundup
        {
            Title = "Roundup",
            Description = "Content summary",
            Author = "Tech Hub Team",
            PublishedDate = DateTimeOffset.UtcNow,
            Categories = ["AI"],
            Tags = ["roundup", "summary", "weekly"],
            TagsNormalized = ["roundup", "summary", "weekly"],
            Permalink = "/roundup.html",
            ViewingMode = "internal",
            RoundupPeriod = period
        };

        // Act & Assert
        roundup.Validate(); // Should not throw
    }
}
