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
        var section = CreateValidSection();

        // Assert
        section.Name.Should().Be("github-copilot");
        section.Title.Should().Be("GitHub Copilot");
        section.Url.Should().Be("/github-copilot");
        section.Tag.Should().Be("GitHub Copilot");
        section.Collections.Select(c => c.Name).Should().Equal("blogs", "news");
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
        var section = A.Section
            .WithName(validName)
            .WithCollections(collection)
            .Build();

        // Assert
        section.Name.Should().Be(validName);
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
        var section = A.Section
            .WithUrl(validUrl)
            .WithCollections(collection)
            .Build();

        // Assert
        section.Url.Should().Be(validUrl);
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
        var section = A.Section
            .WithCollections(collections)
            .Build();

        // Assert
        section.Collections.Select(c => c.Name).Should().Equal("news", "blogs", "videos");
    }

    /// <summary>
    /// Test: Constructor stores every supplied value
    /// Why: Verify no property is dropped or swapped during initialization
    /// </summary>
    [Fact]
    public void Constructor_AllValuesSupplied_StoresEveryProperty()
    {
        // Arrange
        var collection = A.Collection.WithName("videos").Build();

        // Act
        var section = A.Section
            .WithName("machine-learning")
            .WithTitle("Machine Learning")
            .WithDescription("ML resources")
            .WithUrl("/machine-learning")
            .WithTag("ML")
            .WithHideCollectionPages()
            .WithShowHeroBanner()
            .WithCollections(collection)
            .Build();

        // Assert
        section.Name.Should().Be("machine-learning");
        section.Title.Should().Be("Machine Learning");
        section.Description.Should().Be("ML resources");
        section.Url.Should().Be("/machine-learning");
        section.Tag.Should().Be("ML");
        section.HideCollectionPages.Should().BeTrue();
        section.ShowHeroBanner.Should().BeTrue();
        section.Collections.Should().ContainSingle().Which.Should().Be(collection);
    }

    /// <summary>
    /// Test: Constructor defaults the optional display flags to false
    /// Why: Hero banner and hidden collection pages must be opt-in
    /// </summary>
    [Fact]
    public void Constructor_OptionalFlagsOmitted_DefaultToFalse()
    {
        // Arrange & Act
        var section = CreateValidSection();

        // Assert
        section.HideCollectionPages.Should().BeFalse();
        section.ShowHeroBanner.Should().BeFalse();
    }

    /// <summary>
    /// Test: Validate throws ArgumentException when Description or Tag is blank
    /// Why: Description and tag are required section metadata
    /// </summary>
    [Theory]
    [InlineData("description", "", "tag")]
    [InlineData("description", "   ", "tag")]
    [InlineData("tag", "description", "")]
    [InlineData("tag", "description", "   ")]
    public void Constructor_BlankDescriptionOrTag_ThrowsArgumentException(string expectedParameter, string description, string tag)
    {
        // Arrange & Act
        var act = () => A.Section
            .WithDescription(description)
            .WithTag(tag)
            .Build();

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithParameterName(expectedParameter);
    }
}
