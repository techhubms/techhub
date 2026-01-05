using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TechHub.Core.DTOs;
using TechHub.Web.Components;
using Xunit;

namespace TechHub.Web.Tests.Components;

public class CollectionNavTests : TestContext
{
    // No mocks needed - NavigationManager is provided by bUnit TestContext

    [Fact]
    public void Component_DisplaysCollections()
    {
        // Arrange - Component now receives section data directly, no API call
        var collections = new List<CollectionReferenceDto>
        {
            new CollectionReferenceDto { Title = "News", Name = "news", Url = "/ai/news", Description = "" },
            new CollectionReferenceDto { Title = "Videos", Name = "videos", Url = "/ai/videos", Description = "" },
            new CollectionReferenceDto { Title = "Blogs", Name = "blogs", Url = "/ai/blogs", Description = "" }
        };

        var sectionDto = new SectionDto 
        { 
            Name = "ai",
            Title = "AI", 
            Description = "AI Description", 
            Url = "/ai", 
            Category = "AI", 
            BackgroundImage = "/images/ai-bg.jpg", 
            Collections = collections
        };

        // Act
        var cut = RenderComponent<CollectionNav>(parameters => parameters
            .Add(p => p.Section, sectionDto)
            .Add(p => p.SelectedCollection, "all")
            .Add(p => p.IsLoadingContent, false)
            .Add(p => p.OnCollectionChange, EventCallback.Factory.Create<string>(this, _ => { })));

        // Assert
        var buttons = cut.FindAll("button");
        Assert.Equal(4, buttons.Count); // "All" + 3 collections
        
        Assert.Contains("All", buttons[0].TextContent);
        Assert.Contains("News", buttons[1].TextContent);
        Assert.Contains("Videos", buttons[2].TextContent);
        Assert.Contains("Blogs", buttons[3].TextContent);
    }

    [Fact]
    public void Component_HighlightsSelectedCollection()
    {
        // Arrange
        var collections = new List<CollectionReferenceDto>
        {
            new CollectionReferenceDto { Title = "News", Name = "news", Url = "/ai/news", Description = "" }
        };

        var sectionDto = new SectionDto 
        { 
            Name = "ai",
            Title = "AI", 
            Description = "AI Description", 
            Url = "/ai", 
            Category = "AI", 
            BackgroundImage = "/images/ai-bg.jpg", 
            Collections = collections
        };

        // Act
        var cut = RenderComponent<CollectionNav>(parameters => parameters
            .Add(p => p.Section, sectionDto)
            .Add(p => p.SelectedCollection, "news")
            .Add(p => p.IsLoadingContent, false)
            .Add(p => p.OnCollectionChange, EventCallback.Factory.Create<string>(this, _ => { })));

        // Assert
        var buttons = cut.FindAll("button");
        var newsButton = buttons.First(b => b.TextContent.Contains("News"));
        Assert.Contains("active", newsButton.ClassName);
    }

    [Fact]
    public void Component_InvokesCallback_WhenCollectionSelected()
    {
        // Arrange
        string? selectedCollection = null;
        var collections = new List<CollectionReferenceDto>
        {
            new CollectionReferenceDto { Title = "News", Name = "news", Url = "/ai/news", Description = "" }
        };

        var sectionDto = new SectionDto 
        { 
            Name = "ai",
            Title = "AI", 
            Description = "AI Description", 
            Url = "/ai", 
            Category = "AI", 
            BackgroundImage = "/images/ai-bg.jpg", 
            Collections = collections
        };

        // Act
        var cut = RenderComponent<CollectionNav>(parameters => parameters
            .Add(p => p.Section, sectionDto)
            .Add(p => p.SelectedCollection, "all")
            .Add(p => p.IsLoadingContent, false)
            .Add(p => p.OnCollectionChange, EventCallback.Factory.Create<string>(this, value => selectedCollection = value)));

        var buttons = cut.FindAll("button");
        var newsButton = buttons.First(b => b.TextContent.Contains("News"));
        newsButton.Click();

        // Assert
        Assert.Equal("news", selectedCollection);
    }

    [Fact]
    public void Component_UpdatesUrl_WhenCollectionSelected()
    {
        // Arrange
        var collections = new List<CollectionReferenceDto>
        {
            new CollectionReferenceDto { Title = "News", Name = "news", Url = "/ai/news", Description = "" }
        };

        var sectionDto = new SectionDto 
        { 
            Name = "ai",
            Title = "AI", 
            Description = "AI Description", 
            Url = "/ai", 
            Category = "AI", 
            BackgroundImage = "/images/ai-bg.jpg", 
            Collections = collections
        };

        var navMan = Services.GetRequiredService<NavigationManager>();

        // Act
        var cut = RenderComponent<CollectionNav>(parameters => parameters
            .Add(p => p.Section, sectionDto)
            .Add(p => p.SelectedCollection, "all")
            .Add(p => p.IsLoadingContent, false)
            .Add(p => p.OnCollectionChange, EventCallback.Factory.Create<string>(this, _ => { })));

        var buttons = cut.FindAll("button");
        var newsButton = buttons.First(b => b.TextContent.Contains("News"));
        newsButton.Click();

        // Assert - NavigationManager should navigate to /ai/news
        navMan.Uri.Should().EndWith("/ai/news", "clicking News should navigate to /ai/news");
    }

    [Fact]
    public void Component_DisablesButtons_WhenContentIsLoading()
    {
        // Arrange
        var collections = new List<CollectionReferenceDto>
        {
            new CollectionReferenceDto { Title = "News", Name = "news", Url = "/ai/news", Description = "" }
        };

        var sectionDto = new SectionDto 
        { 
            Name = "ai",
            Title = "AI", 
            Description = "AI Description", 
            Url = "/ai", 
            Category = "AI", 
            BackgroundImage = "/images/ai-bg.jpg", 
            Collections = collections
        };

        // Act
        var cut = RenderComponent<CollectionNav>(parameters => parameters
            .Add(p => p.Section, sectionDto)
            .Add(p => p.SelectedCollection, "all")
            .Add(p => p.IsLoadingContent, true)
            .Add(p => p.OnCollectionChange, EventCallback.Factory.Create<string>(this, _ => { })));

        // Assert
        var buttons = cut.FindAll("button");
        Assert.All(buttons, button => Assert.True(button.HasAttribute("disabled")));
    }
}
