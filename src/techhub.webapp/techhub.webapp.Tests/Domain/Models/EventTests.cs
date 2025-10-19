using techhub.webapp.ApiService.Domain.Models;

namespace techhub.webapp.Tests.Domain.Models;

public class EventTests
{
    [Fact]
    public void Validate_WithValidData_SetsCollectionType()
    {
        // Arrange
        var evt = new Event
        {
            Title = "Valid Event",
            Description = "Valid Description",
            Author = "Test Author",
            PublishedDate = DateTimeOffset.UtcNow,
            Categories = ["AI"],
            Tags = ["tag1", "tag2", "tag3"],
            TagsNormalized = ["tag1", "tag2", "tag3"],
            Permalink = "/event.html",
            ViewingMode = "internal",
            StartDate = DateTimeOffset.UtcNow.AddDays(7),
            Location = "Seattle, WA"
        };

        // Act & Assert
        evt.Validate(); // Should not throw
        Assert.Equal("events", evt.CollectionType);
    }

    [Fact]
    public void Validate_WithEmptyLocation_ThrowsException()
    {
        // Arrange
        var evt = new Event
        {
            Title = "Valid Event",
            Description = "Valid Description",
            Author = "Test Author",
            PublishedDate = DateTimeOffset.UtcNow,
            Categories = ["AI"],
            Tags = ["tag1", "tag2", "tag3"],
            TagsNormalized = ["tag1", "tag2", "tag3"],
            Permalink = "/event.html",
            ViewingMode = "internal",
            StartDate = DateTimeOffset.UtcNow.AddDays(7),
            Location = ""
        };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => evt.Validate());
        Assert.Contains("Location cannot be empty", ex.Message);
    }

    [Fact]
    public void Validate_WithEndDateBeforeStartDate_ThrowsException()
    {
        // Arrange
        var evt = new Event
        {
            Title = "Valid Event",
            Description = "Valid Description",
            Author = "Test Author",
            PublishedDate = DateTimeOffset.UtcNow,
            Categories = ["AI"],
            Tags = ["tag1", "tag2", "tag3"],
            TagsNormalized = ["tag1", "tag2", "tag3"],
            Permalink = "/event.html",
            ViewingMode = "internal",
            StartDate = DateTimeOffset.UtcNow.AddDays(7),
            EndDate = DateTimeOffset.UtcNow.AddDays(6), // Before start date
            Location = "Seattle, WA"
        };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => evt.Validate());
        Assert.Contains("EndDate cannot be before StartDate", ex.Message);
    }

    [Fact]
    public void Validate_WithNullEndDate_Succeeds()
    {
        // Arrange
        var evt = new Event
        {
            Title = "Valid Event",
            Description = "Valid Description",
            Author = "Test Author",
            PublishedDate = DateTimeOffset.UtcNow,
            Categories = ["AI"],
            Tags = ["tag1", "tag2", "tag3"],
            TagsNormalized = ["tag1", "tag2", "tag3"],
            Permalink = "/event.html",
            ViewingMode = "internal",
            StartDate = DateTimeOffset.UtcNow.AddDays(7),
            EndDate = null,
            Location = "Online"
        };

        // Act & Assert
        evt.Validate(); // Should not throw
    }

    [Fact]
    public void Validate_WithValidEndDateAfterStartDate_Succeeds()
    {
        // Arrange
        var evt = new Event
        {
            Title = "Valid Event",
            Description = "Valid Description",
            Author = "Test Author",
            PublishedDate = DateTimeOffset.UtcNow,
            Categories = ["AI"],
            Tags = ["tag1", "tag2", "tag3"],
            TagsNormalized = ["tag1", "tag2", "tag3"],
            Permalink = "/event.html",
            ViewingMode = "internal",
            StartDate = DateTimeOffset.UtcNow.AddDays(7),
            EndDate = DateTimeOffset.UtcNow.AddDays(9),
            Location = "Seattle, WA"
        };

        // Act & Assert
        evt.Validate(); // Should not throw
    }
}
