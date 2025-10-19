using techhub.webapp.ApiService.Domain.Models;

namespace techhub.webapp.Tests.Domain.Models;

public class VideoTests
{
    [Fact]
    public void Validate_WithValidData_SetsCollectionType()
    {
        // Arrange
        var video = new Video
        {
            Title = "Valid Video",
            Description = "Valid Description",
            Author = "Test Author",
            PublishedDate = DateTimeOffset.UtcNow,
            Categories = ["AI"],
            Tags = ["tag1", "tag2", "tag3"],
            TagsNormalized = ["tag1", "tag2", "tag3"],
            Permalink = "/video.html",
            ViewingMode = "internal",
            VideoId = "abc123",
            Duration = 300
        };

        // Act & Assert
        video.Validate(); // Should not throw
        Assert.Equal("videos", video.CollectionType);
    }

    [Fact]
    public void Validate_WithEmptyVideoId_ThrowsException()
    {
        // Arrange
        var video = new Video
        {
            Title = "Valid Video",
            Description = "Valid Description",
            Author = "Test Author",
            PublishedDate = DateTimeOffset.UtcNow,
            Categories = ["AI"],
            Tags = ["tag1", "tag2", "tag3"],
            TagsNormalized = ["tag1", "tag2", "tag3"],
            Permalink = "/video.html",
            ViewingMode = "internal",
            VideoId = ""
        };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => video.Validate());
        Assert.Contains("VideoId cannot be empty", ex.Message);
    }

    [Fact]
    public void Validate_WithNegativeDuration_ThrowsException()
    {
        // Arrange
        var video = new Video
        {
            Title = "Valid Video",
            Description = "Valid Description",
            Author = "Test Author",
            PublishedDate = DateTimeOffset.UtcNow,
            Categories = ["AI"],
            Tags = ["tag1", "tag2", "tag3"],
            TagsNormalized = ["tag1", "tag2", "tag3"],
            Permalink = "/video.html",
            ViewingMode = "internal",
            VideoId = "abc123",
            Duration = -1
        };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => video.Validate());
        Assert.Contains("Duration cannot be negative", ex.Message);
    }

    [Fact]
    public void Validate_WithNullDuration_Succeeds()
    {
        // Arrange
        var video = new Video
        {
            Title = "Valid Video",
            Description = "Valid Description",
            Author = "Test Author",
            PublishedDate = DateTimeOffset.UtcNow,
            Categories = ["AI"],
            Tags = ["tag1", "tag2", "tag3"],
            TagsNormalized = ["tag1", "tag2", "tag3"],
            Permalink = "/video.html",
            ViewingMode = "internal",
            VideoId = "abc123",
            Duration = null
        };

        // Act & Assert
        video.Validate(); // Should not throw
    }

    [Fact]
    public void Validate_WithPlansAndGhesSupport_Succeeds()
    {
        // Arrange
        var video = new Video
        {
            Title = "GitHub Copilot Feature",
            Description = "Feature demo",
            Author = "Test Author",
            PublishedDate = DateTimeOffset.UtcNow,
            Categories = ["GitHub Copilot"],
            Tags = ["github copilot", "feature", "demo"],
            TagsNormalized = ["github copilot", "feature", "demo"],
            Permalink = "/ghc-feature.html",
            ViewingMode = "internal",
            VideoId = "abc123",
            Plans = ["Free", "Pro", "Business"],
            GhesSupport = true
        };

        // Act & Assert
        video.Validate(); // Should not throw
    }
}
