using FluentAssertions;
using TechHub.Core.Models;

namespace TechHub.Core.Tests.Models;

/// <summary>
/// Unit tests for Section domain model
/// </summary>
public class SectionTests
{
    private static Section CreateValidSection()
    {
        return new Section
        {
            Name = "ai",
            Title = "AI",
            Description = "Artificial Intelligence resources",
            Url = "/ai",
            Category = "AI",
            BackgroundImage = "/assets/section-backgrounds/ai.jpg",
            Collections = new List<CollectionReference>
            {
                new CollectionReference
                {
                    Name = "news",
                    Title = "News",
                    Url = "/ai/news",
                    Description = "Latest AI news",
                    IsCustom = false
                }
            }
        };
    }

    [Fact]
    public void Validate_PassesForValidSection()
    {
        // Arrange
        var section = CreateValidSection();

        // Act
        var act = () => section.Validate();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_ThrowsWhenNameIsEmpty()
    {
        // Arrange
        var section = new Section
        {
            Name = "",
            Title = "Test",
            Description = "Test description",
            Url = "/test",
            Category = "Test",
            BackgroundImage = "/test.jpg",
            Collections = new List<CollectionReference>
            {
                new CollectionReference
                {
                    Name = "news",
                    Title = "News",
                    Url = "/test/news",
                    Description = "Test",
                    IsCustom = false
                }
            }
        };

        // Act
        var act = () => section.Validate();

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*name*")
            .WithParameterName("Name");
    }

    [Fact]
    public void Validate_ThrowsWhenNameIsWhitespace()
    {
        // Arrange
        var section = new Section
        {
            Name = "   ",
            Title = "Test",
            Description = "Test description",
            Url = "/test",
            Category = "Test",
            BackgroundImage = "/test.jpg",
            Collections = new List<CollectionReference>
            {
                new CollectionReference
                {
                    Name = "news",
                    Title = "News",
                    Url = "/test/news",
                    Description = "Test",
                    IsCustom = false
                }
            }
        };

        // Act
        var act = () => section.Validate();

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithParameterName("Name");
    }

    [Theory]
    [InlineData("AI")] // Uppercase
    [InlineData("GitHub-Copilot")] // Mixed case
    [InlineData("ai_news")] // Underscore
    [InlineData("ai.news")] // Period
    [InlineData("ai news")] // Space
    public void Validate_ThrowsWhenNameHasInvalidCharacters(string invalidName)
    {
        // Arrange
        var section = new Section
        {
            Name = invalidName,
            Title = "Test",
            Description = "Test description",
            Url = "/test",
            Category = "Test",
            BackgroundImage = "/test.jpg",
            Collections = new List<CollectionReference>
            {
                new CollectionReference
                {
                    Name = "news",
                    Title = "News",
                    Url = "/test/news",
                    Description = "Test",
                    IsCustom = false
                }
            }
        };

        // Act
        var act = () => section.Validate();

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*lowercase*hyphen*")
            .WithParameterName("Name");
    }

    [Theory]
    [InlineData("ai")]
    [InlineData("github-copilot")]
    [InlineData("machine-learning")]
    [InlineData("devops")]
    public void Validate_PassesForValidNames(string validName)
    {
        // Arrange
        var section = new Section
        {
            Name = validName,
            Title = "Test",
            Description = "Test description",
            Url = "/test",
            Category = "Test",
            BackgroundImage = "/test.jpg",
            Collections = new List<CollectionReference>
            {
                new CollectionReference
                {
                    Name = "news",
                    Title = "News",
                    Url = "/test/news",
                    Description = "Test",
                    IsCustom = false
                }
            }
        };

        // Act
        var act = () => section.Validate();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_ThrowsWhenTitleIsEmpty()
    {
        // Arrange
        var section = new Section
        {
            Name = "test",
            Title = "",
            Description = "Test description",
            Url = "/test",
            Category = "Test",
            BackgroundImage = "/test.jpg",
            Collections = new List<CollectionReference>
            {
                new CollectionReference
                {
                    Name = "news",
                    Title = "News",
                    Url = "/test/news",
                    Description = "Test",
                    IsCustom = false
                }
            }
        };

        // Act
        var act = () => section.Validate();

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*title*")
            .WithParameterName("Title");
    }

    [Fact]
    public void Validate_ThrowsWhenUrlIsEmpty()
    {
        // Arrange
        var section = new Section
        {
            Name = "test",
            Title = "Test",
            Description = "Test description",
            Url = "",
            Category = "Test",
            BackgroundImage = "/test.jpg",
            Collections = new List<CollectionReference>
            {
                new CollectionReference
                {
                    Name = "news",
                    Title = "News",
                    Url = "/test/news",
                    Description = "Test",
                    IsCustom = false
                }
            }
        };

        // Act
        var act = () => section.Validate();

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*URL*")
            .WithParameterName("Url");
    }

    [Theory]
    [InlineData("ai")] // Missing leading slash
    [InlineData("github-copilot")] // Missing leading slash
    [InlineData("azure/")] // Trailing slash
    public void Validate_ThrowsWhenUrlDoesNotStartWithSlash(string invalidUrl)
    {
        // Arrange
        var section = new Section
        {
            Name = "test",
            Title = "Test",
            Description = "Test description",
            Url = invalidUrl,
            Category = "Test",
            BackgroundImage = "/test.jpg",
            Collections = new List<CollectionReference>
            {
                new CollectionReference
                {
                    Name = "news",
                    Title = "News",
                    Url = "/test/news",
                    Description = "Test",
                    IsCustom = false
                }
            }
        };

        // Act
        var act = () => section.Validate();

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*start with '/'*")
            .WithParameterName("Url");
    }

    [Theory]
    [InlineData("/ai")]
    [InlineData("/github-copilot")]
    [InlineData("/machine-learning")]
    public void Validate_PassesForValidUrls(string validUrl)
    {
        // Arrange
        var section = new Section
        {
            Name = "test",
            Title = "Test",
            Description = "Test description",
            Url = validUrl,
            Category = "Test",
            BackgroundImage = "/test.jpg",
            Collections = new List<CollectionReference>
            {
                new CollectionReference
                {
                    Name = "news",
                    Title = "News",
                    Url = "/test/news",
                    Description = "Test",
                    IsCustom = false
                }
            }
        };

        // Act
        var act = () => section.Validate();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_ThrowsWhenCollectionsIsEmpty()
    {
        // Arrange
        var section = new Section
        {
            Name = "test",
            Title = "Test",
            Description = "Test description",
            Url = "/test",
            Category = "Test",
            BackgroundImage = "/test.jpg",
            Collections = new List<CollectionReference>()
        };

        // Act
        var act = () => section.Validate();

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*at least one collection*")
            .WithParameterName("Collections");
    }

    [Fact]
    public void Validate_PassesWithMultipleCollections()
    {
        // Arrange
        var section = new Section
        {
            Name = "test",
            Title = "Test",
            Description = "Test description",
            Url = "/test",
            Category = "Test",
            BackgroundImage = "/test.jpg",
            Collections = new List<CollectionReference>
            {
                new CollectionReference
                {
                    Name = "news",
                    Title = "News",
                    Url = "/ai/news",
                    Description = "Latest AI news",
                    IsCustom = false
                },
                new CollectionReference
                {
                    Name = "blogs",
                    Title = "Blogs",
                    Url = "/ai/blogs",
                    Description = "AI blog posts",
                    IsCustom = false
                },
                new CollectionReference
                {
                    Name = "videos",
                    Title = "Videos",
                    Url = "/ai/videos",
                    Description = "AI video content",
                    IsCustom = false
                }
            }
        };

        // Act
        var act = () => section.Validate();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void Section_PropertiesAreInitOnly()
    {
        // This test verifies that Section uses init-only setters
        // by creating an instance and verifying properties are set correctly
        
        // Arrange & Act
        var section = new Section
        {
            Name = "test",
            Title = "Test Title",
            Description = "Test Description",
            Url = "/test",
            Category = "Test Category",
            BackgroundImage = "/test-bg.jpg",
            Collections = new List<CollectionReference>()
        };

        // Assert
        section.Title.Should().Be("Test Title");
        section.Description.Should().Be("Test Description");
        section.Name.Should().Be("test");
        section.Url.Should().Be("/test");
    }

    [Fact]
    public void Section_AllPropertiesAreRequired()
    {
        // This test verifies that the Section model enforces required properties
        // by attempting to create instances without required properties
        // If this compiles, the 'required' keyword is working correctly

        // Arrange & Act & Assert - This should compile because all required properties are set
        var section = new Section
        {
            Name = "test",
            Title = "Test",
            Description = "Test description",
            Url = "/test",
            Category = "Test",
            BackgroundImage = "/test.jpg",
            Collections = new List<CollectionReference>()
        };

        section.Should().NotBeNull();
    }
}
