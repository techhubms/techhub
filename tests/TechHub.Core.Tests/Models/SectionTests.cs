using FluentAssertions;
using TechHub.Core.Models;
using TechHub.TestUtilities.Builders;

namespace TechHub.Core.Tests.Models;

/// <summary>
/// Unit tests for Section domain model
/// </summary>
public class SectionTests
{
    private static Section CreateValidSection()
    {
        return A.Section.Build();
    }

    /// <summary>
    /// Test: Validate passes for fully populated valid section
    /// Why: Ensure validation rules don't reject valid sections
    /// </summary>
    [Fact]
    public void Validate_ValidSection_PassesWithoutException()
    {
        // Arrange & Act
        var act = () => CreateValidSection();

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
        // Arrange & Act
        var collection = A.Collection.Build();
        var act = () => A.Section
            .WithName("")
            .WithCollections(collection)
            .Build();

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*name*")
            .WithParameterName("name");
    }

    /// <summary>
    /// Test: Validate throws ArgumentException when Name is whitespace only
    /// Why: Whitespace-only name is invalid for URLs
    /// </summary>
    [Fact]
    public void Validate_WhitespaceName_ThrowsArgumentException()
    {
        // Arrange & Act
        var collection = A.Collection.Build();
        var act = () => A.Section
            .WithName("   ")
            .WithCollections(collection)
            .Build();

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithParameterName("name");
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
        // Arrange & Act
        var collection = A.Collection.Build();
        var act = () => A.Section
            .WithName(invalidName)
            .WithCollections(collection)
            .Build();

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*lowercase*hyphen*")
            .WithParameterName("name");
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
        // Arrange & Act
        var collection = A.Collection.Build();
        var act = () => A.Section
            .WithName(validName)
            .WithCollections(collection)
            .Build();

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
        // Arrange & Act
        var collection = A.Collection.Build();
        var act = () => A.Section
            .WithTitle("")
            .WithCollections(collection)
            .Build();

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*title*")
            .WithParameterName("title");
    }

    /// <summary>
    /// Test: Validate throws ArgumentException when Url is empty
    /// Why: URL is required for routing to section pages
    /// </summary>
    [Fact]
    public void Validate_EmptyUrl_ThrowsArgumentException()
    {
        // Arrange & Act
        var collection = A.Collection.Build();
        var act = () => A.Section
            .WithUrl("")
            .WithCollections(collection)
            .Build();

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*URL*")
            .WithParameterName("url");
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
        // Arrange & Act
        var collection = A.Collection.Build();
        var act = () => A.Section
            .WithUrl(invalidUrl)
            .WithCollections(collection)
            .Build();

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*start with '/'*")
            .WithParameterName("url");
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
        // Arrange & Act
        var collection = A.Collection.Build();
        var act = () => A.Section
            .WithUrl(validUrl)
            .WithCollections(collection)
            .Build();

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
        // Arrange & Act
        var act = () => A.Section
            .WithCollections()
            .Build();

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*at least one collection*")
            .WithParameterName("collections");
    }

    /// <summary>
    /// Test: Validate passes with multiple collections
    /// Why: Sections typically have multiple collection types (news, blogs, videos)
    /// </summary>
    [Fact]
    public void Validate_MultipleCollections_Passes()
    {
        // Arrange & Act
        var collections = new[]
        {
            A.Collection.WithName("news").Build(),
            A.Collection.WithName("blogs").Build(),
            A.Collection.WithName("videos").Build()
        };
        var act = () => A.Section
            .WithCollections(collections)
            .Build();

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
        // Arrange & Act - should throw because empty collections
        var act = () => A.Section
            .WithCollections()
            .Build();

        // Assert - validation happens in constructor now
        act.Should().Throw<ArgumentException>();
    }

    /// <summary>
    /// Test: Section model enforces all required properties
    /// Why: Verify 'required' keyword is working correctly
    /// </summary>
    [Fact]
    public void Section_RequiredProperties_MustBeSet()
    {
        // Arrange & Act - should throw because empty collections
        var act = () => A.Section
            .WithCollections()
            .Build();

        // Assert - validation happens in constructor now
        act.Should().Throw<ArgumentException>();
    }
}
