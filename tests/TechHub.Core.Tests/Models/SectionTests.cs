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
            Collections =
            [
                new CollectionReference
                {
                    Name = "news",
                    Title = "News",
                    Url = "/ai/news",
                    Description = "Latest AI news",
                    IsCustom = false
                }
            ]
        };
    }

    /// <summary>
    /// Test: Validate passes for fully populated valid section
    /// Why: Ensure validation rules don't reject valid sections
    /// </summary>
    [Fact]
    public void Validate_ValidSection_PassesWithoutException()
    {
        // Arrange
        var section = CreateValidSection();

        // Act
        var act = () => section.Validate();

        // Assert
        act.Should().NotThrow();
    }

    /// <summary>
    /// Test: Validate throws ArgumentException when Name is empty string
    /// Why: Section name is required for routing and identification
    /// </summary>
    [Fact]
    public void Validate_EmptyName_ThrowsArgumentException()
    {
        // Arrange
        var section = new Section
        {
            Name = "",
            Title = "Test",
            Description = "Test description",
            Url = "/test",
            Collections =
            [
                new CollectionReference
                {
                    Name = "news",
                    Title = "News",
                    Url = "/test/news",
                    Description = "Test",
                    IsCustom = false
                }
            ]
        };

        // Act
        var act = () => section.Validate();

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*name*")
            .WithParameterName("Name");
    }

    /// <summary>
    /// Test: Validate throws ArgumentException when Name is whitespace only
    /// Why: Whitespace-only name is invalid for URLs
    /// </summary>
    [Fact]
    public void Validate_WhitespaceName_ThrowsArgumentException()
    {
        // Arrange
        var section = new Section
        {
            Name = "   ",
            Title = "Test",
            Description = "Test description",
            Url = "/test",
            Collections =
            [
                new CollectionReference
                {
                    Name = "news",
                    Title = "News",
                    Url = "/test/news",
                    Description = "Test",
                    IsCustom = false
                }
            ]
        };

        // Act
        var act = () => section.Validate();

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithParameterName("Name");
    }

    /// <summary>
    /// Test: Validate throws ArgumentException when Name contains invalid characters
    /// Why: Section names must be lowercase with hyphens only (URL-safe)
    /// </summary>
    [Theory]
    [InlineData("AI")] // Uppercase
    [InlineData("GitHub-Copilot")] // Mixed case
    [InlineData("ai_news")] // Underscore
    [InlineData("ai.news")] // Period
    [InlineData("ai news")] // Space
    public void Validate_InvalidCharactersInName_ThrowsArgumentException(string invalidName)
    {
        // Arrange
        var section = new Section
        {
            Name = invalidName,
            Title = "Test",
            Description = "Test description",
            Url = "/test",
            Collections =
            [
                new CollectionReference
                {
                    Name = "news",
                    Title = "News",
                    Url = "/test/news",
                    Description = "Test",
                    IsCustom = false
                }
            ]
        };

        // Act
        var act = () => section.Validate();

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*lowercase*hyphen*")
            .WithParameterName("Name");
    }

    /// <summary>
    /// Test: Validate passes for valid lowercase-hyphenated names
    /// Why: Verify various valid name formats are accepted
    /// </summary>
    [Theory]
    [InlineData("ai")]
    [InlineData("github-copilot")]
    [InlineData("machine-learning")]
    [InlineData("devops")]
    public void Validate_ValidLowercaseNames_Passes(string validName)
    {
        // Arrange
        var section = new Section
        {
            Name = validName,
            Title = "Test",
            Description = "Test description",
            Url = "/test",
            Collections =
            [
                new CollectionReference
                {
                    Name = "news",
                    Title = "News",
                    Url = "/test/news",
                    Description = "Test",
                    IsCustom = false
                }
            ]
        };

        // Act
        var act = () => section.Validate();

        // Assert
        act.Should().NotThrow();
    }

    /// <summary>
    /// Test: Validate throws ArgumentException when Title is empty
    /// Why: Title is required for section display
    /// </summary>
    [Fact]
    public void Validate_EmptyTitle_ThrowsArgumentException()
    {
        // Arrange
        var section = new Section
        {
            Name = "test",
            Title = "",
            Description = "Test description",
            Url = "/test",
            Collections =
            [
                new CollectionReference
                {
                    Name = "news",
                    Title = "News",
                    Url = "/test/news",
                    Description = "Test",
                    IsCustom = false
                }
            ]
        };

        // Act
        var act = () => section.Validate();

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*title*")
            .WithParameterName("Title");
    }

    /// <summary>
    /// Test: Validate throws ArgumentException when Url is empty
    /// Why: URL is required for routing to section pages
    /// </summary>
    [Fact]
    public void Validate_EmptyUrl_ThrowsArgumentException()
    {
        // Arrange
        var section = new Section
        {
            Name = "test",
            Title = "Test",
            Description = "Test description",
            Url = "",
            Collections =
            [
                new CollectionReference
                {
                    Name = "news",
                    Title = "News",
                    Url = "/test/news",
                    Description = "Test",
                    IsCustom = false
                }
            ]
        };

        // Act
        var act = () => section.Validate();

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*URL*")
            .WithParameterName("Url");
    }

    /// <summary>
    /// Test: Validate throws ArgumentException when URL doesn't start with slash or has trailing slash
    /// Why: URLs must follow format: /section-name (leading slash, no trailing slash)
    /// </summary>
    [Theory]
    [InlineData("ai")] // Missing leading slash
    [InlineData("github-copilot")] // Missing leading slash
    [InlineData("azure/")] // Trailing slash
    public void Validate_InvalidUrlFormat_ThrowsArgumentException(string invalidUrl)
    {
        // Arrange
        var section = new Section
        {
            Name = "test",
            Title = "Test",
            Description = "Test description",
            Url = invalidUrl,
            Collections =
            [
                new CollectionReference
                {
                    Name = "news",
                    Title = "News",
                    Url = "/test/news",
                    Description = "Test",
                    IsCustom = false
                }
            ]
        };

        // Act
        var act = () => section.Validate();

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*start with '/'*")
            .WithParameterName("Url");
    }

    /// <summary>
    /// Test: Validate passes for valid URL formats
    /// Why: Verify various valid URL formats are accepted
    /// </summary>
    [Theory]
    [InlineData("/ai")]
    [InlineData("/github-copilot")]
    [InlineData("/machine-learning")]
    public void Validate_ValidUrlFormats_Passes(string validUrl)
    {
        // Arrange
        var section = new Section
        {
            Name = "test",
            Title = "Test",
            Description = "Test description",
            Url = validUrl,
            Collections =
            [
                new CollectionReference
                {
                    Name = "news",
                    Title = "News",
                    Url = "/test/news",
                    Description = "Test",
                    IsCustom = false
                }
            ]
        };

        // Act
        var act = () => section.Validate();

        // Assert
        act.Should().NotThrow();
    }

    /// <summary>
    /// Test: Validate throws ArgumentException when Collections array is empty
    /// Why: Section must have at least one collection for content display
    /// </summary>
    [Fact]
    public void Validate_EmptyCollections_ThrowsArgumentException()
    {
        // Arrange
        var section = new Section
        {
            Name = "test",
            Title = "Test",
            Description = "Test description",
            Url = "/test",
            Collections = []
        };

        // Act
        var act = () => section.Validate();

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*at least one collection*")
            .WithParameterName("Collections");
    }

    /// <summary>
    /// Test: Validate passes with multiple collections
    /// Why: Sections typically have multiple collection types (news, blogs, videos)
    /// </summary>
    [Fact]
    public void Validate_MultipleCollections_Passes()
    {
        // Arrange
        var section = new Section
        {
            Name = "test",
            Title = "Test",
            Description = "Test description",
            Url = "/test",
            Collections =
            [
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
            ]
        };

        // Act
        var act = () => section.Validate();

        // Assert
        act.Should().NotThrow();
    }

    /// <summary>
    /// Test: Section properties can be set during initialization
    /// Why: Verify record type with init-only properties works as expected
    /// </summary>
    [Fact]
    public void Section_InitOnlyProperties_CanBeSetDuringInitialization()
    {
        // Arrange & Act
        var section = new Section
        {
            Name = "test",
            Title = "Test Title",
            Description = "Test Description",
            Url = "/test",
            Collections = []
        };

        // Assert
        section.Title.Should().Be("Test Title");
        section.Description.Should().Be("Test Description");
        section.Name.Should().Be("test");
        section.Url.Should().Be("/test");
    }

    /// <summary>
    /// Test: Section model enforces all required properties
    /// Why: Verify 'required' keyword is working correctly
    /// </summary>
    [Fact]
    public void Section_RequiredProperties_MustBeSet()
    {
        // Arrange & Act - This compiles because all required properties are set
        var section = new Section
        {
            Name = "test",
            Title = "Test",
            Description = "Test description",
            Url = "/test",
            Collections = []
        };

        section.Should().NotBeNull();
    }
}
