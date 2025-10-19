using techhub.webapp.ApiService.Domain.Models;

namespace techhub.webapp.Tests.Domain.Models;

public class CollectionTests
{
    [Fact]
    public void Validate_WithValidData_Succeeds()
    {
        // Arrange
        var collection = new Collection
        {
            Name = "news",
            Title = "News",
            Description = "News articles",
            Url = "/ai/news.html",
            SectionKey = "ai"
        };

        // Act & Assert
        collection.Validate(); // Should not throw
    }

    [Theory]
    [InlineData("news", true)]
    [InlineData("posts", true)]
    [InlineData("videos", true)]
    [InlineData("community", true)]
    [InlineData("events", true)]
    [InlineData("roundups", true)]
    [InlineData("invalid", false)]
    public void Validate_WithCollectionName_ValidatesCorrectly(string name, bool shouldSucceed)
    {
        // Arrange
        var collection = new Collection
        {
            Name = name,
            Title = "Test Collection",
            Description = "Test description",
            Url = "/test/collection.html",
            SectionKey = "test"
        };

        // Act & Assert
        if (shouldSucceed)
        {
            collection.Validate();
        }
        else
        {
            var ex = Assert.Throws<ArgumentException>(() => collection.Validate());
            Assert.Contains("Name must be one of:", ex.Message);
        }
    }

    [Fact]
    public void Validate_WithEmptyTitle_ThrowsException()
    {
        // Arrange
        var collection = new Collection
        {
            Name = "news",
            Title = "",
            Description = "Test description",
            Url = "/test/news.html",
            SectionKey = "test"
        };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => collection.Validate());
        Assert.Contains("Title cannot be empty", ex.Message);
    }

    [Fact]
    public void Validate_WithEmptyUrl_ThrowsException()
    {
        // Arrange
        var collection = new Collection
        {
            Name = "news",
            Title = "News",
            Description = "Test description",
            Url = "",
            SectionKey = "test"
        };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => collection.Validate());
        Assert.Contains("Url cannot be empty", ex.Message);
    }

    [Fact]
    public void Validate_WithEmptySectionKey_ThrowsException()
    {
        // Arrange
        var collection = new Collection
        {
            Name = "news",
            Title = "News",
            Description = "Test description",
            Url = "/test/news.html",
            SectionKey = ""
        };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => collection.Validate());
        Assert.Contains("SectionKey cannot be empty", ex.Message);
    }
}
