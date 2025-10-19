using techhub.webapp.ApiService.Domain.Models;

namespace techhub.webapp.Tests.Domain.Models;

public class SectionTests
{
    [Fact]
    public void Validate_WithValidData_Succeeds()
    {
        // Arrange
        var section = new Section
        {
            Key = "ai",
            Title = "AI Section",
            Description = "AI content section",
            Url = "/ai",
            Category = "AI",
            Collections =
            [
                new Collection
                {
                    Name = "news",
                    Title = "News",
                    Description = "AI News",
                    Url = "/ai/news.html",
                    SectionKey = "ai"
                }
            ]
        };

        // Act & Assert
        section.Validate(); // Should not throw
    }

    [Theory]
    [InlineData("AI", false)] // Uppercase
    [InlineData("ai_section", false)] // Underscore
    [InlineData("ai section", false)] // Space
    [InlineData("ai123", false)] // Numbers
    [InlineData("ai-section", true)] // Valid
    [InlineData("github-copilot", true)] // Valid
    public void Validate_WithKeyFormat_ValidatesCorrectly(string key, bool shouldSucceed)
    {
        // Arrange
        var section = new Section
        {
            Key = key,
            Title = "Test Section",
            Description = "Test description",
            Url = "/test",
            Category = "AI",
            Collections =
            [
                new Collection
                {
                    Name = "news",
                    Title = "News",
                    Description = "News",
                    Url = "/test/news.html",
                    SectionKey = key
                }
            ]
        };

        // Act & Assert
        if (shouldSucceed)
        {
            section.Validate();
        }
        else
        {
            var ex = Assert.Throws<ArgumentException>(() => section.Validate());
            Assert.Contains("Key must contain only lowercase letters and hyphens", ex.Message);
        }
    }

    [Theory]
    [InlineData("/valid-url", true)]
    [InlineData("invalid-url", false)]
    [InlineData("http://example.com", false)]
    public void Validate_WithUrlFormat_ValidatesCorrectly(string url, bool shouldSucceed)
    {
        // Arrange
        var section = new Section
        {
            Key = "test-section",
            Title = "Test Section",
            Description = "Test description",
            Url = url,
            Category = "AI",
            Collections =
            [
                new Collection
                {
                    Name = "news",
                    Title = "News",
                    Description = "News",
                    Url = "/test/news.html",
                    SectionKey = "test-section"
                }
            ]
        };

        // Act & Assert
        if (shouldSucceed)
        {
            section.Validate();
        }
        else
        {
            var ex = Assert.Throws<ArgumentException>(() => section.Validate());
            Assert.Contains("Url must start with '/'", ex.Message);
        }
    }

    [Theory]
    [InlineData("AI", true)]
    [InlineData("GitHub Copilot", true)]
    [InlineData("ML", true)]
    [InlineData("Azure", true)]
    [InlineData("Coding", true)]
    [InlineData("DevOps", true)]
    [InlineData("Security", true)]
    [InlineData("All", true)]
    [InlineData("Invalid", false)]
    public void Validate_WithCategory_ValidatesCorrectly(string category, bool shouldSucceed)
    {
        // Arrange
        var section = new Section
        {
            Key = "test-section",
            Title = "Test Section",
            Description = "Test description",
            Url = "/test",
            Category = category,
            Collections =
            [
                new Collection
                {
                    Name = "news",
                    Title = "News",
                    Description = "News",
                    Url = "/test/news.html",
                    SectionKey = "test-section"
                }
            ]
        };

        // Act & Assert
        if (shouldSucceed)
        {
            section.Validate();
        }
        else
        {
            var ex = Assert.Throws<ArgumentException>(() => section.Validate());
            Assert.Contains("Category must be one of:", ex.Message);
        }
    }

    [Fact]
    public void Validate_WithEmptyCollections_ThrowsException()
    {
        // Arrange
        var section = new Section
        {
            Key = "test-section",
            Title = "Test Section",
            Description = "Test description",
            Url = "/test",
            Category = "AI",
            Collections = []
        };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => section.Validate());
        Assert.Contains("At least one collection is required", ex.Message);
    }
}
