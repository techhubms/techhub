using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Microsoft.JSInterop.Infrastructure;
using Moq;
using TechHub.Core.DTOs;
using TechHub.Web.Components;
using TechHub.Web.Services;
using Xunit;

namespace TechHub.Web.Tests.Components;

public class CollectionNavTests : TestContext
{
    private readonly Mock<TechHubApiClient> _mockApiClient;
    private readonly Mock<ILogger<CollectionNav>> _mockLogger;
    private readonly Mock<IJSRuntime> _mockJsRuntime;

    public CollectionNavTests()
    {
        _mockApiClient = new Mock<TechHubApiClient>(MockBehavior.Loose, new HttpClient(), Mock.Of<ILogger<TechHubApiClient>>());
        _mockLogger = new Mock<ILogger<CollectionNav>>();
        _mockJsRuntime = new Mock<IJSRuntime>();
        
        Services.AddSingleton(_mockApiClient.Object);
        Services.AddSingleton(_mockLogger.Object);
        Services.AddSingleton(_mockJsRuntime.Object);
    }

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
        Assert.Equal(4, buttons.Count); // "Everything" + 3 collections
        
        Assert.Contains("Everything", buttons[0].TextContent);
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

        _mockJsRuntime
            .Setup(x => x.InvokeAsync<IJSVoidResult>(
                "history.pushState",
                It.IsAny<object[]>()))
            .ReturnsAsync(Mock.Of<IJSVoidResult>());

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

        _mockJsRuntime
            .Setup(x => x.InvokeAsync<IJSVoidResult>(
                "history.pushState",
                It.Is<object[]>(args => 
                    args.Length == 3 && 
                    (string)args[2] == "/ai/news")))
            .ReturnsAsync(Mock.Of<IJSVoidResult>())
            .Verifiable();

        // Act
        var cut = RenderComponent<CollectionNav>(parameters => parameters
            .Add(p => p.Section, sectionDto)
            .Add(p => p.SelectedCollection, "all")
            .Add(p => p.IsLoadingContent, false)
            .Add(p => p.OnCollectionChange, EventCallback.Factory.Create<string>(this, _ => { })));

        var buttons = cut.FindAll("button");
        var newsButton = buttons.First(b => b.TextContent.Contains("News"));
        newsButton.Click();

        // Assert
        _mockJsRuntime.Verify();
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
