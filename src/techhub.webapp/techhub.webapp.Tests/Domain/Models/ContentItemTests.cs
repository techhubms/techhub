using techhub.webapp.ApiService.Domain.Models;

namespace techhub.webapp.Tests.Domain.Models;

public class ContentItemTests
{
    [Fact]
    public void Validate_WithValidData_Succeeds()
    {
        // Arrange
        var item = new TestContentItem
        {
            Title = "Valid Title",
            Description = "Valid Description",
            Author = "Test Author",
            PublishedDate = DateTimeOffset.UtcNow,
            Categories = ["AI"],
            Tags = ["tag1", "tag2", "tag3"],
            TagsNormalized = ["tag1", "tag2", "tag3"],
            Permalink = "/2025-01-01-valid-title.html",
            ViewingMode = "external",
            CanonicalUrl = "https://example.com/article",
            CollectionType = "news"
        };

        // Act & Assert
        item.Validate(); // Should not throw
    }

    [Fact]
    public void Validate_WithTitleTooLong_ThrowsException()
    {
        // Arrange
        var item = new TestContentItem
        {
            Title = new string('a', 201), // 201 characters
            Description = "Valid Description",
            Author = "Test Author",
            PublishedDate = DateTimeOffset.UtcNow,
            Categories = ["AI"],
            Tags = ["tag1", "tag2", "tag3"],
            TagsNormalized = ["tag1", "tag2", "tag3"],
            Permalink = "/test.html",
            ViewingMode = "internal",
            CollectionType = "news"
        };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => item.Validate());
        Assert.Contains("Title cannot exceed 200 characters", ex.Message);
    }

    [Fact]
    public void Validate_WithDescriptionTooLong_ThrowsException()
    {
        // Arrange
        var item = new TestContentItem
        {
            Title = "Valid Title",
            Description = new string('a', 501), // 501 characters
            Author = "Test Author",
            PublishedDate = DateTimeOffset.UtcNow,
            Categories = ["AI"],
            Tags = ["tag1", "tag2", "tag3"],
            TagsNormalized = ["tag1", "tag2", "tag3"],
            Permalink = "/test.html",
            ViewingMode = "internal",
            CollectionType = "news"
        };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => item.Validate());
        Assert.Contains("Description cannot exceed 500 characters", ex.Message);
    }

    [Fact]
    public void Validate_WithEmptyCategories_ThrowsException()
    {
        // Arrange
        var item = new TestContentItem
        {
            Title = "Valid Title",
            Description = "Valid Description",
            Author = "Test Author",
            PublishedDate = DateTimeOffset.UtcNow,
            Categories = [],
            Tags = ["tag1", "tag2", "tag3"],
            TagsNormalized = ["tag1", "tag2", "tag3"],
            Permalink = "/test.html",
            ViewingMode = "internal",
            CollectionType = "news"
        };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => item.Validate());
        Assert.Contains("At least one category is required", ex.Message);
    }

    [Fact]
    public void Validate_WithFewerThanThreeTags_ThrowsException()
    {
        // Arrange
        var item = new TestContentItem
        {
            Title = "Valid Title",
            Description = "Valid Description",
            Author = "Test Author",
            PublishedDate = DateTimeOffset.UtcNow,
            Categories = ["AI"],
            Tags = ["tag1", "tag2"], // Only 2 tags
            TagsNormalized = ["tag1", "tag2"],
            Permalink = "/test.html",
            ViewingMode = "internal",
            CollectionType = "news"
        };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => item.Validate());
        Assert.Contains("At least 3 tags are required", ex.Message);
    }

    [Fact]
    public void Validate_WithMismatchedTagCounts_ThrowsException()
    {
        // Arrange
        var item = new TestContentItem
        {
            Title = "Valid Title",
            Description = "Valid Description",
            Author = "Test Author",
            PublishedDate = DateTimeOffset.UtcNow,
            Categories = ["AI"],
            Tags = ["tag1", "tag2", "tag3"],
            TagsNormalized = ["tag1", "tag2"], // Mismatched count
            Permalink = "/test.html",
            ViewingMode = "internal",
            CollectionType = "news"
        };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => item.Validate());
        Assert.Contains("Tags and TagsNormalized must have the same count", ex.Message);
    }

    [Fact]
    public void Validate_WithInvalidPermalinkFormat_ThrowsException()
    {
        // Arrange
        var item = new TestContentItem
        {
            Title = "Valid Title",
            Description = "Valid Description",
            Author = "Test Author",
            PublishedDate = DateTimeOffset.UtcNow,
            Categories = ["AI"],
            Tags = ["tag1", "tag2", "tag3"],
            TagsNormalized = ["tag1", "tag2", "tag3"],
            Permalink = "invalid-permalink", // Missing leading slash
            ViewingMode = "internal",
            CollectionType = "news"
        };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => item.Validate());
        Assert.Contains("Permalink must start with '/'", ex.Message);
    }

    [Fact]
    public void Validate_WithInvalidViewingMode_ThrowsException()
    {
        // Arrange
        var item = new TestContentItem
        {
            Title = "Valid Title",
            Description = "Valid Description",
            Author = "Test Author",
            PublishedDate = DateTimeOffset.UtcNow,
            Categories = ["AI"],
            Tags = ["tag1", "tag2", "tag3"],
            TagsNormalized = ["tag1", "tag2", "tag3"],
            Permalink = "/test.html",
            ViewingMode = "invalid", // Invalid value
            CollectionType = "news"
        };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => item.Validate());
        Assert.Contains("ViewingMode must be 'internal' or 'external'", ex.Message);
    }

    [Fact]
    public void Validate_WithExternalViewingModeButNoCanonicalUrl_ThrowsException()
    {
        // Arrange
        var item = new TestContentItem
        {
            Title = "Valid Title",
            Description = "Valid Description",
            Author = "Test Author",
            PublishedDate = DateTimeOffset.UtcNow,
            Categories = ["AI"],
            Tags = ["tag1", "tag2", "tag3"],
            TagsNormalized = ["tag1", "tag2", "tag3"],
            Permalink = "/test.html",
            ViewingMode = "external",
            CanonicalUrl = null, // Missing for external
            CollectionType = "news"
        };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => item.Validate());
        Assert.Contains("CanonicalUrl is required for external viewing mode", ex.Message);
    }

    // Test helper class
    private class TestContentItem : ContentItem
    {
        public TestContentItem()
        {
            CollectionType = "test";
        }
    }
}
