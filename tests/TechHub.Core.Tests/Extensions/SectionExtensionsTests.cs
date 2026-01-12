using FluentAssertions;
using TechHub.Core.Extensions;
using TechHub.Core.Models;

namespace TechHub.Core.Tests.Extensions;

/// <summary>
/// Unit tests for SectionExtensions mapping methods
/// </summary>
public class SectionExtensionsTests
{
    private static Section CreateTestSection()
    {
        return new Section
        {
            Name = "ai",
            Title = "AI",
            Description = "Artificial Intelligence resources",
            Url = "/ai",
            BackgroundImage = "/assets/section-backgrounds/ai.jpg",
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
                }
            ]
        };
    }

    [Fact]
    public void ToDto_Section_ConvertsAllProperties()
    {
        // Arrange
        var section = CreateTestSection();

        // Act
        var dto = section.ToDto();

        // Assert
        dto.Name.Should().Be("ai");
        dto.Title.Should().Be("AI");
        dto.Description.Should().Be("Artificial Intelligence resources");
        dto.Url.Should().Be("/ai");
        dto.Name.Should().Be("ai");
        dto.BackgroundImage.Should().Be("/assets/section-backgrounds/ai.jpg");
        dto.Collections.Should().HaveCount(2);
    }

    [Fact]
    public void ToDto_Section_ThrowsArgumentNullException_WhenSectionIsNull()
    {
        // Arrange
        Section? section = null;

        // Act
        var act = () => section!.ToDto();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ToDto_Section_ConvertsCollections()
    {
        // Arrange
        var section = CreateTestSection();

        // Act
        var dto = section.ToDto();

        // Assert
        dto.Collections.Should().HaveCount(2);
        dto.Collections[0].Name.Should().Be("news");
        dto.Collections[0].Title.Should().Be("News");
        dto.Collections[0].Url.Should().Be("/ai/news");
        dto.Collections[0].Description.Should().Be("Latest AI news");
        dto.Collections[0].IsCustom.Should().BeFalse();

        dto.Collections[1].Name.Should().Be("blogs");
        dto.Collections[1].Title.Should().Be("Blogs");
    }

    [Fact]
    public void ToDto_CollectionReference_ConvertsAllProperties()
    {
        // Arrange
        var collection = new CollectionReference
        {
            Name = "videos",
            Title = "Videos",
            Url = "/github-copilot/videos",
            Description = "Video tutorials",
            IsCustom = false
        };

        // Act
        var dto = collection.ToDto();

        // Assert
        dto.Name.Should().Be("videos");
        dto.Title.Should().Be("Videos");
        dto.Url.Should().Be("/github-copilot/videos");
        dto.Description.Should().Be("Video tutorials");
        dto.IsCustom.Should().BeFalse();
    }

    [Fact]
    public void ToDto_CollectionReference_ThrowsArgumentNullException_WhenCollectionIsNull()
    {
        // Arrange
        CollectionReference? collection = null;

        // Act
        var act = () => collection!.ToDto();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ToDto_CollectionReference_HandlesCustomCollections()
    {
        // Arrange
        var collection = new CollectionReference
        {
            Name = "custom",
            Title = "Custom Collection",
            Url = "/ai/custom",
            Description = "Custom content",
            IsCustom = true
        };

        // Act
        var dto = collection.ToDto();

        // Assert
        dto.IsCustom.Should().BeTrue();
    }

    [Fact]
    public void ToDtos_Sections_ConvertsMultipleSections()
    {
        // Arrange
        var sections = new List<Section>
        {
            CreateTestSection(),
            new Section
            {
                Name = "github-copilot",
                Title = "GitHub Copilot",
                Description = "GitHub Copilot resources",
                Url = "/github-copilot",
                BackgroundImage = "/assets/section-backgrounds/github-copilot.jpg",
                Collections =
                [
                    new CollectionReference
                    {
                        Name = "news",
                        Title = "News",
                        Url = "/github-copilot/news",
                        Description = "Latest news",
                        IsCustom = false
                    }
                ]
            }
        };

        // Act
        var dtos = sections.ToDtos();

        // Assert
        dtos.Should().HaveCount(2);
        dtos[0].Name.Should().Be("ai");
        dtos[0].Title.Should().Be("AI");
        dtos[1].Name.Should().Be("github-copilot");
        dtos[1].Title.Should().Be("GitHub Copilot");
    }

    [Fact]
    public void ToDtos_Sections_ReturnsEmptyList_WhenNoSections()
    {
        // Arrange
        var sections = Enumerable.Empty<Section>();

        // Act
        var dtos = sections.ToDtos();

        // Assert
        dtos.Should().BeEmpty();
    }

    [Fact]
    public void ToDto_Section_HandlesEmptyCollections()
    {
        // Arrange - Note: In real scenarios, sections should have at least one collection
        // This test verifies the conversion handles edge cases
        var section = new Section
        {
            Name = "ai",
            Title = "AI",
            Description = "Artificial Intelligence resources",
            Url = "/ai",
            BackgroundImage = "/assets/section-backgrounds/ai.jpg",
            Collections = []
        };

        // Act
        var dto = section.ToDto();

        // Assert
        dto.Collections.Should().BeEmpty();
    }
}
